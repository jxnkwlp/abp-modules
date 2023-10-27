using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Account.Settings;
using Passingwind.Abp.Identity;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Passingwind.Abp.Account;

//[ApiExplorerSettings(IgnoreApi = true)]

[Area(AccountRemoteServiceConsts.ModuleName)]
[Route("auth/external")]
public class AccountExternalAuthController : AbpController
{
    protected IJsonSerializer JsonSerializer { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }
    protected IAuthenticationSchemeProvider SchemeProvider { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IExternalUserProvider ExternalUserProvider { get; }
    protected IOptions<AccountExternalLoginOptions> ExternalLoginOptions { get; }
    protected IAccountExternalProvider AccountExternalCallbackProvider { get; }
    protected SettingProvider SettingProvider { get; }

    public AccountExternalAuthController(
        IJsonSerializer jsonSerializer,
        SignInManager<IdentityUser> signInManager,
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<IdentityOptions> identityOptions,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentityUserManager userManager,
        IExternalUserProvider externalUserProvider,
        IOptions<AccountExternalLoginOptions> externalLoginOptions,
        IAccountExternalProvider accountExternalCallbackProvider,
        SettingProvider settingProvider)
    {
        JsonSerializer = jsonSerializer;
        SignInManager = signInManager;
        SchemeProvider = schemeProvider;
        IdentityOptions = identityOptions;
        IdentitySecurityLogManager = identitySecurityLogManager;
        UserManager = userManager;
        ExternalUserProvider = externalUserProvider;
        ExternalLoginOptions = externalLoginOptions;
        AccountExternalCallbackProvider = accountExternalCallbackProvider;
        SettingProvider = settingProvider;
    }

    [HttpGet("{provider}/login")]
    public virtual IActionResult Login([NotNull] string provider, string? returnUrl = null, string? returnUrlHash = null)
    {
        var redirectUrl = Url.Action("callback", values: new { returnUrl, returnUrlHash });

        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        return Challenge(properties, provider);
    }

    [HttpGet("callback")]
    public virtual async Task<IActionResult> CallbackAsync(string returnUrl = "", string returnUrlHash = "", string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            Logger.LogWarning("External login callback error: {0}", remoteError);

            throw new BusinessException(AccountErrorCodes.ExternalLoginRemoteError);
        }

        await IdentityOptions.SetAsync();

        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");

            throw new AbpAuthorizationException("External login info is not available");
        }

        Logger.LogInformation("Received external login. provider: {LoginProvider}, key: {ProviderKey}", loginInfo.LoginProvider, loginInfo.ProviderKey);

        if (ExternalLoginOptions.Value.LogClaims)
        {
            var logClaimsString = JsonSerializer.Serialize(loginInfo.Principal.Claims.Select(x => new { x.Type, x.Value }));
            Logger.LogDebug("Received external login principal claims: \n{LogClaimsString}", logClaimsString);
        }

        var loginInfoContext = new AccountExternalCallbackLoginInfoContext(loginInfo);
        await AccountExternalCallbackProvider.LoginInfoReceivedAsync(loginInfoContext);

        if (loginInfoContext.Handled && loginInfoContext.Result != null)
        {
            return loginInfoContext.Result;
        }

        // signin 
        var signInResult = await ExternalLoginSignInAsync(loginInfo);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
            Action = "Login" + signInResult,
            UserName = loginInfo.ProviderKey,
        });

        var signInContext = new AccountExternalCallbackSignInContext(signInResult);
        await AccountExternalCallbackProvider.SignInAsync(signInContext);
        if (signInContext.Handled && signInContext.Result != null)
        {
            return signInContext.Result;
        }

        var user = await MatchUserAsync(loginInfo);

        var userSigninContext = new AccountExternalCallbackUserSignInContext(loginInfo, user);
        await AccountExternalCallbackProvider.UserSignInAsync(userSigninContext);
        if (userSigninContext.Handled && userSigninContext.Result != null)
        {
            return userSigninContext.Result;
        }

        user = userSigninContext.User;

        if (user == null)
        {
            throw new BusinessException(AccountErrorCodes.ExternalLoginUserNotFound);
        }

        // TODO: two-factory check!
        // sign in
        await SignInManager.SignInAsync(user, false);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogActionConsts.LoginSucceeded,
            UserName = user.Name
        });

        return RedirectSafely(returnUrl, returnUrlHash);
    }

    protected virtual async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo loginInfo)
    {
        var bypassTwofactor = await SettingProvider.GetAsync<bool>(AccountSettingNames.ExternalLogin.BypassTwofactory);

        var signInResult = await SignInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: bypassTwofactor
        );

        Logger.LogInformation("External login use provider key '{ProviderKey}', name '{LoginProvider}', signin result: {SignInResult}", loginInfo.ProviderKey, loginInfo.LoginProvider, signInResult);

        return signInResult;
    }

    protected virtual async Task<IdentityUser?> MatchUserAsync(ExternalLoginInfo loginInfo)
    {
        // find user
        var user = await ExternalUserProvider.FindUserAsync(loginInfo.Principal, loginInfo.LoginProvider, loginInfo.ProviderKey);

        // create user
        if (user == null)
        {
            Logger.LogDebug("External login not match any user. provider key '{ProviderKey}', name '{LoginProvider}'", loginInfo.ProviderKey, loginInfo.LoginProvider);

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
            Logger.LogDebug("External login user found. user name '{UserName}', provider key '{ProviderKey}', name '{LoginProvider}'", user.UserName, loginInfo.ProviderKey, loginInfo.LoginProvider);

            // update user
            user = await ExternalUserProvider.UpdateUserAsync(user, loginInfo.Principal);

            // add login if not exists
            var login = await UserManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (login == null)
            {
                await UserManager.AddLoginAsync(user, new UserLoginInfo(loginInfo.LoginProvider, loginInfo.ProviderKey, loginInfo.ProviderDisplayName));
            }
        }

        return user;
    }
}
