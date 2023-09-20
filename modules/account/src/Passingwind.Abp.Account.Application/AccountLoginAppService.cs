using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.AspNetCore;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Passingwind.Abp.Account;

[AllowAnonymous]
public class AccountLoginAppService : AccountAppBaseService, IAccountLoginAppService
{
    protected SignInManager SignInManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public AccountLoginAppService(
        SignInManager signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager securityLogManager,
        IOptions<IdentityOptions> identityOptions)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        SecurityLogManager = securityLogManager;
        IdentityOptions = identityOptions;
    }

    /// <inheritdoc/>
    public virtual async Task<AccountLoginResultDto> CheckPasswordAsync(AccountLoginRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var identityUser = await FindUserAsync(input.UserNameOrEmailAddress);

        if (identityUser == null)
        {
            return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPasswordOrToken);
        }

        return GetAccountLoginResult(await SignInManager.CheckPasswordSignInAsync(identityUser, input.Password, true));
    }

    /// <inheritdoc/>
    public virtual async Task<AccountLoginResultDto> LoginAsync(AccountLoginRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckLocalLoginAsync();

        var user = await FindUserAsync(input.UserNameOrEmailAddress);

        if (user == null)
        {
            return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPasswordOrToken);
        }

        var signInResult = await SignInManager.PasswordSignInAsync(
            user,
            input.Password,
            input.RememberMe,
            true
        );

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = ToIdentitySecurityLogAction(signInResult),
            UserName = user.UserName,
        });

        return GetAccountLoginResult(signInResult);
    }

    /// <inheritdoc/>
    public virtual async Task<AccountLoginResultDto> LoginWith2faAsync(AccountLoginWith2FaRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync() ?? throw new AbpAuthorizationException("Unable to load two-factor authentication user.");

        var authenticatorCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var isRememberBrowserEnabled = await SettingProvider.IsTrueAsync(IdentitySettingNamesV2.Twofactor.IsRememberBrowserEnabled);

        var signInResult = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, input.RememberMe, input.RememberMachine && isRememberBrowserEnabled);

        await UserManager.GetUserIdAsync(user);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = ToIdentitySecurityLogAction(signInResult),
            UserName = user.UserName,
        });

        return GetAccountLoginResult(signInResult);
    }

    /// <inheritdoc/>
    public virtual async Task<AccountLoginResultDto> LoginWithRecoveryCodeAsync(AccountLoginWithRecoveryCodeRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync() ?? throw new AbpAuthorizationException("Unable to load two-factor authentication user.");

        var recoveryCode = input.RecoveryCode.Replace(" ", string.Empty);

        var signInResult = await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        await UserManager.GetUserIdAsync(user);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = signInResult.ToIdentitySecurityLogAction(),
            UserName = user.UserName,
        });

        return GetAccountLoginResult(signInResult);
    }

    /// <inheritdoc/>
    [IgnoreAntiforgeryToken]
    public virtual async Task LogoutAsync()
    {
        await IdentityOptions.SetAsync();

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.Logout
        });

        await SignInManager.SignOutAsync();
    }

    public virtual async Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetRequiresChangePasswordUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        if (await UserManager.HasPasswordAsync(user))
        {
            await UserManager.RemovePasswordAsync(user);
        }

        (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();

        user.SetShouldChangePasswordOnNextLogin(false);

        (await UserManager.UpdateAsync(user)).CheckErrors();
    }

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var verificationCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (is2faTokenValid)
        {
            await UserManager.SetTwoFactorEnabledAsync(user, true);

            await UserManager.GetUserIdAsync(user);

            Logger.LogInformation("User with id '{id}' has enabled 2FA with an authenticator app.", user.Id);

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
                Action = "AuthenticatorEnabled",
                UserName = user.UserName,
            });

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return new AccountAuthenticatorRecoveryCodesResultDto
            {
                RecoveryCodes = recoveryCodes!.ToArray(),
            };
        }

        throw new BusinessException(AccountErrorCodes.TwoFactorCodeValidFailed);
    }

    public virtual async Task<AccountHasAuthenticatorResultDto> HasAuthenticatorAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);

        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);

        return new AccountHasAuthenticatorResultDto
        {
            Enabled = !string.IsNullOrEmpty(unformattedKey) && enabled,
        };
    }

    public virtual async Task<AccountAuthenticatorInfoDto> GetAuthenticatorInfoAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);

        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);

        if (string.IsNullOrEmpty(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        var sharedKey = FormatKey(unformattedKey!);

        var userName = await UserManager.GetUserNameAsync(user);
        var authenticatorUri = await GenerateAuthenticatorQrCodeUri(userName!, unformattedKey!);

        enabled = !string.IsNullOrEmpty(unformattedKey) && enabled;

        if (enabled)
        {
            return new AccountAuthenticatorInfoDto { Enabled = true };
        }

        return new AccountAuthenticatorInfoDto
        {
            Enabled = enabled,
            Key = unformattedKey,
            FormatKey = sharedKey,
            Uri = authenticatorUri,
        };
    }

    public virtual async Task<ListResultDto<AccountExternalAuthenticationSchameDto>> GetExternalAuthenticationsAsync()
    {
        var schames = await SignInManager.GetExternalAuthenticationSchemesAsync();

        return new ListResultDto<AccountExternalAuthenticationSchameDto>(schames.Select(x => new AccountExternalAuthenticationSchameDto()
        {
            Name = x.Name,
            DisplayName = x.DisplayName,
        }).ToList());
    }

    protected virtual async Task<IdentityUser?> FindUserAsync(string userNameOrEmailAddress)
    {
        var user = await UserManager.FindByNameAsync(userNameOrEmailAddress);
        if (user != null)
        {
            return user;
        }

        var requireUniqueEmail = IdentityOptions.Value.User.RequireUniqueEmail;

        // if an email address
        if (ValidationHelper.IsValidEmailAddress(userNameOrEmailAddress) && requireUniqueEmail)
        {
            return await UserManager.FindByEmailAsync(userNameOrEmailAddress);
        }
        // TODO: can select account to login ?
        //

        return null;
    }

    private static AccountLoginResultDto GetAccountLoginResult(SignInResult result)
    {
        if (result.Succeeded)
        {
            return new AccountLoginResultDto(AccountLoginResultType.Success);
        }

        if (result.IsLockedOut)
        {
            return new AccountLoginResultDto(AccountLoginResultType.LockedOut);
        }

        if (result.RequiresTwoFactor)
        {
            return new AccountLoginResultDto(AccountLoginResultType.RequiresTwoFactor);
        }

        if (result.IsNotAllowed)
        {
            return new AccountLoginResultDto(AccountLoginResultType.NotAllowed);
        }

        if (result is AbpSignInResult abpSignInResult && abpSignInResult.RequiresChangePassword)
        {
            return new AccountLoginResultDto(AccountLoginResultType.RequiresChangePassword);
        }

        return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPasswordOrToken);
    }

    protected virtual async Task CheckLocalLoginAsync()
    {
        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
        {
            throw new BusinessException(AccountErrorCodes.LocalLoginDisabled);
        }
    }

    protected static string ToIdentitySecurityLogAction(SignInResult signInResult)
    {
        if (signInResult is AbpSignInResult abpSignInResult)
        {
            return $"Login{abpSignInResult}";
        }

        return signInResult.ToIdentitySecurityLogAction();
    }
}
