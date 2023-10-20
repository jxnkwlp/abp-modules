using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Identity.AspNetCore;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpSignInManager))]
public class SignInManager : AbpSignInManager, IScopedDependency
{
    protected IdentityUserManager IdentityUserManager { get; }

    public SignInManager(
        Volo.Abp.Identity.IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<AbpIdentityOptions> options,
        ISettingProvider settingProvider,
        IdentityUserManager identityUserManager) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation, options, settingProvider)
    {
        IdentityUserManager = identityUserManager;
    }

    protected override async Task<SignInResult?> PreSignInCheck(Volo.Abp.Identity.IdentityUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning("The user is not active therefore cannot login! (username: \"{0}\", id:\"{1}\")", user.UserName, user.Id);
            return SignInResult.NotAllowed;
        }

        if (!await CanSignInAsync(user))
        {
            return SignInResult.NotAllowed;
        }

        if (await IsLockedOut(user))
        {
            return await LockedOut(user);
        }

        return null;
    }

    protected virtual async Task<SignInResult> PostSignInCheck(IdentityUser user, SignInResult signInResult)
    {
        if (await IdentityUserManager.ShouldChangePasswordAsync(user))
        {
            return AbpSignInResult.ChangePasswordRequired;
        }

        return signInResult;
    }

    // TODO
    // will be override on net8 and review this code
    public virtual async Task<bool> IsTwoFactorEnabledAsync(IdentityUser user)
    {
        var behaviour = await SettingProvider.GetEnumValueAsync<IdentityTwofactoryBehaviour>(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour);

        if (behaviour == IdentityTwofactoryBehaviour.Disabled)
            return false;

        if (behaviour == IdentityTwofactoryBehaviour.Forced && UserManager.SupportsUserTwoFactor)
            return true;

        return UserManager.SupportsUserTwoFactor
            && await UserManager.GetTwoFactorEnabledAsync(user)
            && (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
    }

    public virtual async Task<bool> ShouldChangePasswordAsync(IdentityUser user)
    {
        return await IdentityUserManager.ShouldChangePasswordAsync(user);
    }

    // TODO
    // review this when upgrade to .net8
    protected override async Task<SignInResult> SignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false)
    {
        if (await ShouldChangePasswordAsync(user))
        {
            var userId = await UserManager.GetUserIdAsync(user);
            await Context.SignInAsync(IdentityV2Constants.RequiresChangePasswordScheme, StoreChangePasswordInfo(userId, loginProvider));

            return AbpSignInResult.ChangePasswordRequired;
        }

        if (!bypassTwoFactor && await IsTwoFactorEnabledAsync(user))
        {
            if (!await IsTwoFactorClientRememberedAsync(user))
            {
                // Store the userId for use after two factor check
                var userId = await UserManager.GetUserIdAsync(user);
                await Context.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, StoreTwoFactorInfo(userId, loginProvider));

                return SignInResult.TwoFactorRequired;
            }
        }

        // Cleanup external cookie
        if (loginProvider != null)
        {
            await Context.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        if (loginProvider == null)
        {
            await SignInWithClaimsAsync(user, isPersistent, new Claim[] { new Claim("amr", "pwd") });
        }
        else
        {
            await SignInAsync(user, isPersistent, loginProvider);
        }

        return SignInResult.Success;

        // return await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }

    public override async Task SignOutAsync()
    {
        await base.SignOutAsync();
        await Context.SignOutAsync(IdentityV2Constants.RequiresChangePasswordScheme);
    }

    /// <summary>
    ///  Gets the <typeparamref name="IdentityUser"/> for the current requires change password authentication login
    /// </summary>
    public virtual async Task<IdentityUser?> GetChangePasswordAuthenticationUserAsync()
    {
        var result = await Context.AuthenticateAsync(IdentityV2Constants.RequiresChangePasswordScheme);
        if (result?.Principal == null)
        {
            return null;
        }

        var userId = result.Principal.FindFirstValue(ClaimTypes.Name);
        if (userId == null)
        {
            return null;
        }

        return await UserManager.FindByIdAsync(userId);
    }

    /// <summary>
    /// Creates a claims principal for the specified 2fa information.
    /// </summary>
    /// <param name="userId">The user whose is logging in via 2fa.</param>
    /// <param name="loginProvider">The 2fa provider.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> containing the user 2fa information.</returns>
    internal static ClaimsPrincipal StoreTwoFactorInfo(string userId, string? loginProvider)
    {
        var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, userId));
        if (loginProvider != null)
        {
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
        }
        return new ClaimsPrincipal(identity);
    }

    /// <summary>
    ///  Creates a claims principal for the specified user information that requires change password.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="loginProvider"></param>
    internal static ClaimsPrincipal StoreChangePasswordInfo(string userId, string? loginProvider)
    {
        var identity = new ClaimsIdentity(IdentityV2Constants.RequiresChangePasswordScheme);

        identity.AddClaim(new Claim(ClaimTypes.Name, userId));
        if (loginProvider != null)
        {
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
        }

        return new ClaimsPrincipal(identity);
    }
}
