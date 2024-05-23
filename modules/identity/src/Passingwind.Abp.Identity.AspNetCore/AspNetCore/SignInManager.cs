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

    protected override async Task<SignInResult?> PreSignInCheck(IdentityUser user)
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

    /// <summary>
    ///  Post action when user signin
    /// </summary>
    protected virtual Task<SignInResult> PostSignInCheckAsync(IdentityUser user, SignInResult signInResult)
    {
        return Task.FromResult(signInResult);
    }

    /// <summary>
    ///  Check if the user has need change password before login
    /// </summary>
    public virtual async Task<bool> ShouldChangePasswordAsync(IdentityUser user)
    {
        return await IdentityUserManager.ShouldChangePasswordAsync(user);
    }

    /// <inheritdoc />
    public override async Task<bool> IsTwoFactorEnabledAsync(IdentityUser user)
    {
        var behaviour = await SettingProvider.GetEnumValueAsync<IdentityTwofactoryBehaviour>(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour);

        if (behaviour == IdentityTwofactoryBehaviour.Disabled)
            return false;

        if (behaviour == IdentityTwofactoryBehaviour.Forced && UserManager.SupportsUserTwoFactor)
            return true;

        return await base.IsTwoFactorEnabledAsync(user);
    }

    /// <summary>
    ///  This is to call the protection method <see cref="SignInOrTwoFactorAsync"/>
    /// </summary>
    public virtual async Task<SignInResult> DirectSignInAsync(IdentityUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false, bool bypassChangePassword = false)
    {
        return await SignInOrTwoFactorAsync(
            user,
            isPersistent: isPersistent,
            loginProvider: loginProvider,
            bypassTwoFactor: bypassTwoFactor,
            bypassChangePassword: bypassChangePassword);
    }

    /// <inheritdoc />
    protected virtual async Task<SignInResult> SignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false, bool bypassChangePassword = false)
    {
        SignInResult result = SignInResult.Failed;

        if (!bypassChangePassword && await ShouldChangePasswordAsync(user))
        {
            var userId = await UserManager.GetUserIdAsync(user);
            await Context.SignInAsync(IdentityV2Constants.ChangePasswordUserIdScheme, StoreChangePasswordInfo(userId, loginProvider));

            result = AbpSignInResult.ChangePasswordRequired;
        }
        else
        {
            result = await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
        }

        return await PostSignInCheckAsync(user, result);
    }

    /// <inheritdoc />
    protected override async Task<SignInResult> SignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false)
    {
        return await SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor, false);
    }

    /// <inheritdoc />
    public override async Task SignOutAsync()
    {
        await base.SignOutAsync();
        await Context.SignOutAsync(IdentityV2Constants.ChangePasswordUserIdScheme);
    }

    /// <summary>
    ///  Gets the <typeparamref name="IdentityUser"/> for the current requires change password authentication login
    /// </summary>
    public virtual async Task<IdentityUser?> GetChangePasswordAuthenticationUserAsync()
    {
        var result = await Context.AuthenticateAsync(IdentityV2Constants.ChangePasswordUserIdScheme);
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
    ///  Creates a claims principal for the specified user information that requires change password.
    /// </summary>
    internal static ClaimsPrincipal StoreChangePasswordInfo(string userId, string? loginProvider)
    {
        var identity = new ClaimsIdentity(IdentityV2Constants.ChangePasswordUserIdScheme);

        identity.AddClaim(new Claim(ClaimTypes.Name, userId));
        if (loginProvider != null)
        {
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
        }

        return new ClaimsPrincipal(identity);
    }
}
