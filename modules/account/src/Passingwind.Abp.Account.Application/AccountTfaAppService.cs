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

    public virtual async Task<AccountTfaDto> GetAsync()
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var enabled = await UserManager.GetTwoFactorEnabledAsync(user);
        var isMachineRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user);

        return new AccountTfaDto
        {
            Enabled = enabled,
            IsMachineRemembered = enabled && isMachineRemembered,
        };
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

    public virtual async Task<ListResultDto<string>> GetProvidersAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var list = await UserManager.GetValidTwoFactorProvidersAsync(user);

        return new ListResultDto<string>(list.ToList());
    }

    public virtual async Task SendTokenAsync(string provider)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var validProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);

        if (!validProviders.Contains(provider))
        {
            throw new UserFriendlyException("Invalid token provider");
        }

        var token = await UserManager.GenerateTwoFactorTokenAsync(user, provider);

        await AccountTwoFactorTokenSender.SendAsync(user, provider, token);

        Logger.LogInformation("User with id '{id}' has been generated new token '{token}' for provider '{provider}'.", user.Id, token, provider);

        await AccountTwoFactorTokenSender.SendAsync(user, provider, token);
    }

    public virtual async Task<AccountVerifyTokenResultDto> VerifyTokenAsync(string provider, AccountTfaVerifyTokenRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var valid = await UserManager.VerifyTwoFactorTokenAsync(user, provider, input.Token);

        Logger.LogInformation("User with id '{id}' use provider '{provider}' verify two-factor token '{token}' result: {valid}.", user.Id, input.Token, provider, valid);

        return new AccountVerifyTokenResultDto
        {
            Valid = valid,
        };
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
            Key = unformattedKey,
            FormatKey = sharedKey,
            Uri = authenticatorUri,
        };
    }

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> UpdateAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

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

    public virtual async Task<AccountAuthenticatorRecoveryCodesResultDto> GenerateAuthenticatorRecoveryCodesAsync()
    {
        await IdentityOptions.SetAsync();

        await CheckTfaDisabledAsync();

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

    public virtual async Task ResetAuthenticatorAsync(AccountAuthenticatorCodeVerifyRequestDto input)
    {
        await IdentityOptions.SetAsync();

        await CheckTfaForcedAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.Id!.Value);

        var verificationCode = input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2faTokenValid)
        {
            throw new BusinessException(AccountErrorCodes.TwoFactorCodeValidFailed);
        }

        await UserManager.SetTwoFactorEnabledAsync(user, false);
        await UserManager.ResetAuthenticatorKeyAsync(user);
        await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        Logger.LogInformation("User with id '{id}' has reset their authentication app key.", user.Id);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
            Action = "AuthenticatorKeyReset",
            UserName = user.UserName,
        });

        await SignInManager.RefreshSignInAsync(user);
    }

    protected virtual async Task<IdentityTwofactoryBehaviour> GetTwofactoryBehaviourAsync()
    {
        return await SettingProvider.GetEnumValueAsync<IdentityTwofactoryBehaviour>(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour);
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
