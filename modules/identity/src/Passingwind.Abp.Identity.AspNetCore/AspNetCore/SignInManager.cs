using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

    protected override async Task<SignInResult> SignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string? loginProvider = null, bool bypassTwoFactor = false)
    {
        if (await IdentityUserManager.ShouldChangePasswordAsync(user))
        {
            Logger.LogWarning("The user should change password! (username: \"{0}\", id:\"{1}\")", user.UserName, user.Id);

            var claimIdentity = new ClaimsIdentity(MyIdentityConstants.ApplicationPartialScheme);

            var userId = await UserManager.GetUserIdAsync(user);

            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, userId));
            if (loginProvider != null)
            {
                claimIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
            }

            await Context.SignInAsync(MyIdentityConstants.ApplicationPartialScheme, new ClaimsPrincipal(claimIdentity));

            return AbpSignInResult.ChangePasswordRequired;
        }

        return await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }

    public override async Task SignOutAsync()
    {
        await base.SignOutAsync();
        await Context.SignOutAsync(MyIdentityConstants.ApplicationPartialScheme);
    }

    public virtual async Task<IdentityUser?> GetPartialAuthenticationUserAsync()
    {
        var result = await Context.AuthenticateAsync(MyIdentityConstants.ApplicationPartialScheme);
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
}
