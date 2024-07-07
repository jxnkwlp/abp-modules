using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Account.Events;
using Passingwind.Abp.Account.Settings;
using Passingwind.Abp.Identity;
using Passingwind.Abp.Identity.AspNetCore;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;

namespace Passingwind.Abp.Account;

[AllowAnonymous]
public class AccountExternalAppService : AccountAppBaseService, IAccountExternalAppService
{
    protected HttpContext? HttpContext { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected SignInManager SignInManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IExternalUserProvider ExternalUserProvider { get; }
    protected IOptions<AccountExternalLoginOptions> ExternalLoginOptions { get; set; }
    protected ILocalEventBus LocalEventBus { get; }

    public AccountExternalAppService(
        SignInManager signInManager,
        IHttpContextAccessor httpContextAccessor,
        IOptions<IdentityOptions> identityOptions,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentityUserManager userManager,
        IJsonSerializer jsonSerializer,
        IExternalUserProvider externalUserProvider,
        IOptions<AccountExternalLoginOptions> externalLoginOptions,
        ILocalEventBus localEventBus)
    {
        SignInManager = signInManager;
        HttpContext = httpContextAccessor.HttpContext;
        IdentityOptions = identityOptions;
        IdentitySecurityLogManager = identitySecurityLogManager;
        UserManager = userManager;
        JsonSerializer = jsonSerializer;
        ExternalUserProvider = externalUserProvider;
        ExternalLoginOptions = externalLoginOptions;
        LocalEventBus = localEventBus;
    }

    public virtual async Task LoginAsync([NotNull] string provider, string? redirectUrl = null)
    {
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl ?? "/");
        properties.Items["scheme"] = provider;

        if (HttpContext == null)
            throw new ArgumentException($"{nameof(HttpContext)} is null");

        await HttpContext.ChallengeAsync(provider, properties);
    }

    /// <summary>
    ///  When external login callback
    /// </summary>
    /// <exception cref="BusinessException"></exception>
    /// <exception cref="AbpAuthorizationException">When the external login info not exists </exception>
    public virtual async Task<AccountExternalLoginResultDto> CallbackAsync([NotNull] AccountExternalLoginCallbackDto input)
    {
        if (!string.IsNullOrWhiteSpace(input.RemoteError))
        {
            Logger.LogWarning("External login callback error: {0}", input.RemoteError);

            throw new BusinessException(AccountErrorCodes.ExternalLoginRemoteError);
        }

        await IdentityOptions.SetAsync();

        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");

            throw new AbpAuthorizationException("External login info is not available");
        }

        Logger.LogInformation("Received external login callback. provider: {LoginProvider}, key: {ProviderKey}", loginInfo.LoginProvider, loginInfo.ProviderKey);

        if (ExternalLoginOptions.Value.LogClaims)
        {
            var logClaimsString = JsonSerializer.Serialize(loginInfo.Principal.Claims.Select(x => new { x.Type, x.Value }));
            Logger.LogDebug("Received external login principal claims: \n{LogClaimsString}", logClaimsString);
        }

        var result = await ExternalLoginSignInAsync(loginInfo);

        if (result.ToString() != SignInResult.Failed.ToString())
        {
            return new AccountExternalLoginResultDto(GetAccountLoginResultType(result))
            {
                RedirectUrl = input.ReturnUrl,
            };
        }

        var user = await MatchUserAsync(loginInfo);

        if (user == null)
        {
            throw new BusinessException(AccountErrorCodes.ExternalLoginUserNotFound);
        }

        // try login again
        result = await ExternalLoginSignInAsync(loginInfo);

        if (result.ToString() != SignInResult.Failed.ToString())
        {
            return new AccountExternalLoginResultDto(GetAccountLoginResultType(result))
            {
                RedirectUrl = input.ReturnUrl,
            };
        }

        throw new BusinessException(AccountErrorCodes.ExternalLoginUserNotFound);
    }

    protected virtual async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo loginInfo)
    {
        var bypassTwofactor = await SettingProvider.GetAsync<bool>(AccountSettingNames.ExternalLogin.BypassTwofactory);

        var result = await SignInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: bypassTwofactor
        );

        if (result.ToString() != SignInResult.Failed.ToString())
        {
            var user = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

            await LocalEventBus.PublishAsync(new UserLoginEvent(user!.Id, UserLoginEvent.ExternalLogin), onUnitOfWorkComplete: true);

            Logger.LogInformation("User use provider key '{ProviderKey}' logged in with '{LoginProvider}' provider.", loginInfo.ProviderKey, loginInfo.LoginProvider);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = user.UserName,
            });
        }

        return result;
    }

    protected virtual async Task<IdentityUser?> MatchUserAsync(ExternalLoginInfo loginInfo)
    {
        // find user
        var user = await ExternalUserProvider.FindUserAsync(loginInfo.Principal, loginInfo.LoginProvider, loginInfo.ProviderKey);

        // create user
        if (user == null)
        {
            var canCreateUser = await SettingProvider.GetAsync<bool>(AccountSettingNames.ExternalLogin.AutoCreateUser);

            if (canCreateUser)
            {
                user = await ExternalUserProvider.CreateUserAsync(
                    principal: loginInfo.Principal,
                    loginProvider: loginInfo.LoginProvider,
                    providerKey: loginInfo.ProviderKey,
                    loginDisplayName: loginInfo.ProviderDisplayName,
                    generateUserName: true);

                Logger.LogInformation("User with name '{UserName}' created by external login with '{LoginProvider}' provider.", user.UserName, loginInfo.LoginProvider);

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                    Action = "Created",
                    UserName = user.Name
                });
            }
            else
            {
                return null;
            }
        }
        else
        {
            // update user
            user = await ExternalUserProvider.UpdateUserAsync(user, loginInfo.Principal);
        }

        return user;
    }
}
