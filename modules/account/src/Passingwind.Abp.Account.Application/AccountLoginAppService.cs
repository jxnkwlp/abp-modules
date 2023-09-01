using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Settings;
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
            return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPassword);
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
            return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPassword);
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
    public async Task<AccountLoginResultDto> LoginWith2faAsync(AccountLoginWith2FaRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync() ?? throw new AbpAuthorizationException("Unable to load two-factor authentication user.");

        var authenticatorCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var signInResult = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, input.RememberMe, input.RememberMachine);

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
    public async Task<AccountLoginResultDto> LoginWithRecoveryCodeAsync(AccountLoginWithRecoveryCodeRequestDto input)
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

    public async Task ChangePasswordAsync(AccountRequiredChangePasswordRequestDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await SignInManager.GetPartialAuthenticationUserAsync();

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

        return new AccountLoginResultDto(AccountLoginResultType.InvalidUserNameOrPassword);
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
