using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Account.Settings;
using Passingwind.Abp.Identity.AspNetCore;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp;
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
    protected IAccountTwoFactorTokenSender AccountTwoFactorTokenSender { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected SignInManager SignInManager { get; }
    protected IdentityUserManager UserManager { get; }

    public AccountLoginAppService(
        SignInManager signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager securityLogManager,
        IAccountTwoFactorTokenSender accountTwoFactorTokenSender,
        IOptions<IdentityOptions> identityOptions)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        SecurityLogManager = securityLogManager;
        AccountTwoFactorTokenSender = accountTwoFactorTokenSender;
        IdentityOptions = identityOptions;
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
    public virtual async Task<AccountLoginResultDto> LoginWithAuthenticatorRecoveryCodeAsync(AccountLoginWithAuthenticatorRecoveryCodeRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

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
    public virtual async Task<AccountLoginResultDto> LoginWithTfaAsync(string provider, AccountLoginWithTfaRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        _ = await UserManager.GetUserIdAsync(user);

        var isRememberBrowserEnabled = await SettingProvider.IsTrueAsync(IdentitySettingNamesV2.Twofactor.IsRememberBrowserEnabled);

        SignInResult signInResult;

        if (provider == IdentityOptions.Value.Tokens.AuthenticatorTokenProvider)
        {
            var authenticatorCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            signInResult = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, input.RememberMe, isRememberBrowserEnabled);
        }
        else
        {
            signInResult = await SignInManager.TwoFactorSignInAsync(provider, input.Code, input.RememberMe, isRememberBrowserEnabled);
        }

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = ToIdentitySecurityLogAction(signInResult),
            UserName = user.UserName,
        });

        return GetAccountLoginResult(signInResult);
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

    public virtual async Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input)
    {
        await IdentityOptions.SetAsync();

        if (!await SettingProvider.GetAsync<bool>(AccountSettingNames.General.EnableChangePasswordOnLogin))
        {
            throw new BusinessException(AccountErrorCodes.LoginChangePasswordDisabled);
        }

        var user = await SignInManager.GetChangePasswordAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        if (await UserManager.HasPasswordAsync(user))
        {
            await UserManager.RemovePasswordAsync(user);
        }

        (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();

        user.SetShouldChangePasswordOnNextLogin(false);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.ChangePassword,
            UserName = user.UserName,
        });

        // signout all schames
        await SignInManager.SignOutAsync();
    }

    public virtual async Task<AccountTFaStateDto> GetTfaStatusAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var enabled = UserManager.SupportsUserTwoFactor && await UserManager.GetTwoFactorEnabledAsync(user);

        var validTwoFactorProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        return new AccountTFaStateDto
        {
            Enabled = enabled,
            Providers = validTwoFactorProviders.ToArray(),
        };
    }

    public virtual async Task SendTfaTokenAsync(string provider)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var validProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        if (!validProviders.Contains(provider))
        {
            throw new UserFriendlyException("Invalid token provider");
        }

        var token = await UserManager.GenerateTwoFactorTokenAsync(user, provider);

        Logger.LogInformation("User with id '{id}' has been generated new token '{token}' for provider '{provider}'.", user.Id, token, provider);

        await AccountTwoFactorTokenSender.SendAsync(user, provider, token);
    }

    public virtual async Task<AccountVerifyTokenResultDto> VerifyTfaTokenAsync(string provider, AccountLoginVerifyTwoFactorTokenDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var validProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        if (!validProviders.Contains(provider))
        {
            throw new UserFriendlyException("Invalid token provider");
        }

        var valid = await UserManager.VerifyTwoFactorTokenAsync(user, provider, input.Token);

        Logger.LogInformation("User with id '{id}' use provider '{provider}' verify two-factor token '{token}' result: {valid}.", user.Id, input.Token, provider, valid);

        return new AccountVerifyTokenResultDto
        {
            Valid = valid,
        };
    }

    public virtual async Task<AccountHasAuthenticatorResultDto> HasAuthenticatorAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);

        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        var recoveryCodesCount = await UserManager.CountRecoveryCodesAsync(user);

        return new AccountHasAuthenticatorResultDto
        {
            Enabled = !string.IsNullOrEmpty(unformattedKey) && recoveryCodesCount > 0 && enabled,
        };
    }

    public virtual async Task<AccountAuthenticatorInfoDto> GetAuthenticatorInfoAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        if (!await SettingProvider.GetAsync<bool>(AccountSettingNames.General.EnableAuthenticatorSetupOnLogin))
        {
            throw new BusinessException(AccountErrorCodes.LoginAuthenticatorSetupDisabled);
        }

        if (!await SettingProvider.GetAsync<bool>(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled))
        {
            throw new BusinessException(AccountErrorCodes.AuthenticatorDisabled);
        }

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);

        var recoveryCodesCount = await UserManager.CountRecoveryCodesAsync(user);

        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);

        if (string.IsNullOrEmpty(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        var userName = await UserManager.GetUserNameAsync(user);
        var authenticatorUri = await GenerateAuthenticatorQrCodeUri(userName!, unformattedKey!);

        var sharedKey = FormatKey(unformattedKey!);

        enabled = !string.IsNullOrEmpty(unformattedKey) && recoveryCodesCount > 0 && enabled;

        if (enabled)
        {
            return new AccountAuthenticatorInfoDto { Enabled = true };
        }

        return new AccountAuthenticatorInfoDto
        {
            Enabled = false,
            Identifier = userName,
            Key = unformattedKey,
            FormatKey = sharedKey,
            Uri = authenticatorUri,
        };
    }

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> VerifyAuthenticatorToken(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new AbpAuthorizationException();

        if (!await SettingProvider.GetAsync<bool>(AccountSettingNames.General.EnableAuthenticatorSetupOnLogin))
        {
            throw new BusinessException(AccountErrorCodes.LoginAuthenticatorSetupDisabled);
        }

        if (!await SettingProvider.GetAsync<bool>(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled))
        {
            throw new BusinessException(AccountErrorCodes.AuthenticatorDisabled);
        }

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

            // signout all schames
            await SignInManager.SignOutAsync();

            return new AccountAuthenticatorRecoveryCodesResultDto
            {
                RecoveryCodes = recoveryCodes!.ToArray(),
            };
        }

        throw new BusinessException(AccountErrorCodes.TwoFactorCodeValidFailed);
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
}
