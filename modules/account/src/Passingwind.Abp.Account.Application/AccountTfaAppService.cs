using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity;
using Passingwind.Abp.Identity.AspNetCore;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountTfaAppService : AccountAppBaseService, IAccountTfaAppService
{
    protected SignInManager SignInManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IAccountTwoFactorTokenSender AccountTwoFactorTokenSender { get; }

    public AccountTfaAppService(
        SignInManager signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager securityLogManager,
        IOptions<IdentityOptions> identityOptions,
        IAccountTwoFactorTokenSender accountTwoFactorTokenSender)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        SecurityLogManager = securityLogManager;
        IdentityOptions = identityOptions;
        AccountTwoFactorTokenSender = accountTwoFactorTokenSender;
    }

    public virtual async Task<ListResultDto<string>> GetAllProvidersAsync()
    {
        await IdentityOptions.SetAsync();

        var list = IdentityOptions.Value.Tokens.ProviderMap.Keys.ToList();

        return new ListResultDto<string>(list);
    }

    public virtual async Task<AccountTfaDto> GetAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);
        var isMachineRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user);

        var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);

        return new AccountTfaDto
        {
            Enabled = enabled,
            IsMachineRemembered = enabled && isMachineRemembered,
            Providers = providers,
        };
    }

    public virtual async Task<ListResultDto<string>> GetProvidersAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var list = await UserManager.GetValidTwoFactorProvidersAsync(user);

        return new ListResultDto<string>(list.ToList());
    }

    public virtual async Task ForgetClientAsync()
    {
        await IdentityOptions.SetAsync();

        _ = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        await SignInManager.ForgetTwoFactorClientAsync();

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = "ForgetClient",
            UserName = CurrentUser.UserName,
        });
    }
    public virtual async Task DisableAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaForcedAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        if (await UserManager.GetTwoFactorEnabledAsync(user))
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, false)).CheckErrors();

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
                Action = "Disabled",
                UserName = user.UserName,
            });
        }

        Logger.LogInformation("User with id '{id}' has disabled 2fa.", user.Id);
    }

    public virtual async Task<AccountAuthenticatorInfoDto> GetAuthenticatorAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        await CheckAuthenticatorDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

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

        return new AccountAuthenticatorInfoDto
        {
            Enabled = !string.IsNullOrEmpty(unformattedKey) && enabled,
            UserName = userName,
            Key = unformattedKey,
            FormatKey = sharedKey,
            Uri = authenticatorUri,
        };
    }

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        await CheckAuthenticatorDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

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

    public virtual async Task<AccountVerifyTokenResultDto> VerifyAuthenticatorTokenAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        await CheckAuthenticatorDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var verificationCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        return new AccountVerifyTokenResultDto { Valid = is2faTokenValid };
    }

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> GenerateAuthenticatorRecoveryCodesAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        await CheckAuthenticatorDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var codes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        Logger.LogInformation("User with id '{id}' has generated new 2FA recovery codes.", user.Id);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = "AuthenticatorResetRecoveryCodes",
            UserName = user.UserName,
        });

        return new AccountAuthenticatorRecoveryCodesResultDto
        {
            RecoveryCodes = codes!.ToArray(),
        };
    }

    public virtual async Task RemoveAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaForcedAsync();

        await CheckAuthenticatorDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var verificationCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2faTokenValid)
        {
            throw new BusinessException(AccountErrorCodes.TwoFactorCodeValidFailed);
        }

        var validProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        if (validProviders.Count == 1)
        {
            await UserManager.SetTwoFactorEnabledAsync(user, false);
        }

        await UserManager.RemoveAuthenticatorAsync(user);

        Logger.LogInformation("User with id '{id}' has reset their authentication app key.", user.Id);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = "AuthenticatorRemoved",
            UserName = user.UserName,
        });

        await SignInManager.RefreshSignInAsync(user);
    }

    protected virtual async Task<IdentityTwofactoryBehaviour> GetTwofactoryBehaviourAsync()
    {
        return await SettingProvider.GetEnumValueAsync<IdentityTwofactoryBehaviour>(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour);
    }

    protected virtual async Task CheckAuthenticatorDisabledAsync()
    {
        if (!await SettingProvider.GetAsync<bool>(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled))
        {
            throw new BusinessException(AccountErrorCodes.AuthenticatorDisabled);
        }
    }

    protected virtual async Task CheckTfaDisabledAsync()
    {
        var behaviour = await GetTwofactoryBehaviourAsync();

        if (behaviour == IdentityTwofactoryBehaviour.Disabled)
        {
            throw new BusinessException(AccountErrorCodes.TwoFactorDisabled);
        }
    }
    protected virtual async Task CheckTfaForcedAsync()
    {
        var behaviour = await GetTwofactoryBehaviourAsync();

        if (behaviour == IdentityTwofactoryBehaviour.Forced)
        {
            throw new BusinessException(AccountErrorCodes.TwoFactorCannotDisabled);
        }
    }
}
