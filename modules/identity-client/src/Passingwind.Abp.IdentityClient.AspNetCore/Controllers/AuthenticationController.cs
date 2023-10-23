using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient.Identity;
using Passingwind.Abp.IdentityClient.Options;
using Passingwind.Abp.IdentityClient.Saml2;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Json;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClient.Controllers;

[Area("IdentityClient")]
[Route("auth/external")]
public class AuthenticationController : AbpController
{
    protected IJsonSerializer JsonSerializer { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }
    protected IAuthenticationSchemeProvider SchemeProvider { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected IdentityClientOption IdentityClientOptions { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IExternalUserProvider ExternalUserProvider { get; }
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected ISaml2OptionBuilder Saml2ConfigurationCreator { get; }
    protected IExternalLoginEventProvider ExternalLoginEventProvider { get; }

    public AuthenticationController(
        SignInManager<IdentityUser> signInManager,
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<IdentityOptions> identityOptions,
        IOptions<IdentityClientOption> identityClientOptions,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentityUserManager userManager,
        IExternalUserProvider externalUserProvider,
        IIdentityClientRepository identityClientRepository,
        ISaml2OptionBuilder saml2ConfigurationCreator,
        IJsonSerializer jsonSerializer,
        IExternalLoginEventProvider externalLoginEventProvider)
    {
        SignInManager = signInManager;
        SchemeProvider = schemeProvider;
        IdentityOptions = identityOptions;
        IdentityClientOptions = identityClientOptions.Value;
        IdentitySecurityLogManager = identitySecurityLogManager;
        UserManager = userManager;
        ExternalUserProvider = externalUserProvider;
        IdentityClientRepository = identityClientRepository;
        Saml2ConfigurationCreator = saml2ConfigurationCreator;
        JsonSerializer = jsonSerializer;
        ExternalLoginEventProvider = externalLoginEventProvider;
    }

    [AllowAnonymous]
    [HttpGet("{provider}/login")]
    public async Task<IActionResult> LoginAsync([NotNull] string provider, string? returnUrl = null, string? returnUrlHash = null)
    {
        var identityClient = await IdentityClientRepository.FindByProviderNameAsync(provider);

        if (identityClient == null || identityClient?.IsEnabled != true)
            return HandleError(new ExternalLoginCallbackErrorDto { Error = true, Description = $"The provider '{provider}' not exist or disabled." });

        // TODO: Check Tenant ???

        var redirectUrl = Url.Action("callback", values: new { returnUrl, returnUrlHash });
        var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        properties.Items["scheme"] = provider;

        var context = new ExternalLoginRedirectToIdentityProviderContext(HttpContext)
        {
            AuthenticationProperties = properties
        };
        await ExternalLoginEventProvider.RedirectToIdentityProviderAsync(context);

        if (context.Handled && context.Result != null)
        {
            return context.Result;
        }

        return Challenge(properties, provider);
    }

    [AllowAnonymous]
    [HttpGet("callback")]
    public async Task<IActionResult> CallbackAsync(string returnUrl = "", string returnUrlHash = "", string? remoteError = null)
    {
        var messageReceivedContext = new ExternalLoginMessageReceivedContext(HttpContext)
        {
            ReturnUrl = returnUrl,
            RemoteError = remoteError,
        };
        await ExternalLoginEventProvider.MessageReceivedAsync(messageReceivedContext);

        returnUrl = messageReceivedContext.ReturnUrl;
        if (messageReceivedContext.Handled && messageReceivedContext.Result != null)
        {
            return messageReceivedContext.Result;
        }

        if (remoteError != null)
        {
            Logger.LogWarning($"External login callback error: {remoteError}");

            return HandleError(new ExternalLoginCallbackErrorDto { Error = true, Description = remoteError });
        }

        await IdentityOptions.SetAsync();

        var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            Logger.LogWarning("External login info is not available");

            return HandleError(new ExternalLoginCallbackErrorDto { Error = true, Description = "External login info is not available" });
        }

        var externalLoginInfoReceivedContext = new ExternalLoginInfoReceivedContext(HttpContext)
        {
            ReturnUrl = returnUrl,
            ExternalLoginInfo = loginInfo,
        };
        await ExternalLoginEventProvider.LoginInfoReceivedAsync(externalLoginInfoReceivedContext);

        loginInfo = externalLoginInfoReceivedContext.ExternalLoginInfo;
        returnUrl = externalLoginInfoReceivedContext.ReturnUrl;

        if (externalLoginInfoReceivedContext.Handled && externalLoginInfoReceivedContext.Result != null)
        {
            return externalLoginInfoReceivedContext.Result;
        }

        Logger.LogInformation("Received external login. provider: {0}, key: {1}", loginInfo.LoginProvider, loginInfo.ProviderKey);

        var logClaimsString = JsonSerializer.Serialize(loginInfo.Principal.Claims.Select(x => new { x.Type, x.Value }));
        Logger.LogDebug("External login principal claims: {0}", logClaimsString);

        // check is debug mode
        var identityClient = await IdentityClientRepository.FindByProviderNameAsync(loginInfo.LoginProvider);

        // TODO: Check Tenant ???
        // 
        if (identityClient != null)
        {
            if (identityClient.IsDebugMode)
            {
                return Ok(loginInfo.Principal.Claims.Select(x => new { x.Type, x.Value }));
            }
        }

        var result = await SignInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        var signInContext = new ExternalLoginSignInContext(HttpContext, loginInfo)
        {
            SignInResult = result,
            ReturnUrl = returnUrl,
        };
        await ExternalLoginEventProvider.SignInAsync(signInContext);

        result = signInContext.SignInResult;
        returnUrl = signInContext.ReturnUrl;

        if (signInContext.Handled && signInContext.Result != null)
        {
            return signInContext.Result;
        }

        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = "Login" + result
            });
        }

        if (result.IsLockedOut)
        {
            Logger.LogWarning("External login callback error: user is locked out!");

            return HandleError(new ExternalLoginCallbackErrorDto
            {
                Error = true,
                Description = "user is locked out",
                Result = ExternalLoginResultType.LockedOut
            });
        }
        else if (result.IsNotAllowed)
        {
            Logger.LogWarning("External login callback error: user is not allowed!");

            return HandleError(new ExternalLoginCallbackErrorDto
            {
                Error = true,
                Description = "user is not allowed",
                Result = ExternalLoginResultType.NotAllowed
            });
        }
        else if (result.RequiresTwoFactor)
        {
            Logger.LogWarning("External login callback error: user login requires two factory");

            return HandleError(new ExternalLoginCallbackErrorDto
            {
                Error = true,
                Description = "user login requires two factory",
                Result = ExternalLoginResultType.RequiresTwoFactor
            });
        }
        else
        {
            // use not found. 
        }

        // try find user
        var user = await ExternalUserProvider.FindUserAsync(loginInfo.Principal, loginInfo.LoginProvider, loginInfo.ProviderKey);

        // login success and user exists.
        if (result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = user?.Name
            });

            return RedirectSafely(returnUrl, returnUrlHash);
        }

        // create user & sign in 
        if (user == null)
        {
            user = await ExternalUserProvider.CreateUserAsync(loginInfo.Principal, loginInfo.LoginProvider, loginInfo.ProviderKey);
        }
        else
        {
            user = await ExternalUserProvider.UpdateUserAsync(user, loginInfo.Principal, loginInfo.LoginProvider, loginInfo.ProviderKey);
        }

        var userSignInContext = new ExternalLoginUserSignInContext(HttpContext, loginInfo)
        {
            IdentityUser = user,
            ReturnUrl = returnUrl,
        };
        await ExternalLoginEventProvider.UserSignInAsync(userSignInContext);

        user = userSignInContext.IdentityUser;
        returnUrl = userSignInContext.ReturnUrl;

        if (userSignInContext.Handled && userSignInContext.Result != null)
        {
            return userSignInContext.Result;
        }

        // sign in
        await SignInManager.SignInAsync(user, false);

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.IdentityExternal,
            Action = result.ToIdentitySecurityLogAction(),
            UserName = user.Name
        });

        return RedirectSafely(returnUrl, returnUrlHash);
    }

    protected IActionResult HandleError(ExternalLoginCallbackErrorDto error)
    {
        if (IdentityClientOptions.RedirectToErrorPage && !string.IsNullOrWhiteSpace(IdentityClientOptions.ErrorPageUrl))
        {
            return Redirect($"{IdentityClientOptions.ErrorPageUrl}?error_description={error.Description}");
        }

        return BadRequest(error);
    }
}
