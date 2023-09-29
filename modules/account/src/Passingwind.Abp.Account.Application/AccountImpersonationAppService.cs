using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity;
using Passingwind.Abp.Identity.AspNetCore;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountImpersonationAppService : AccountAppBaseService, IAccountImpersonationAppService
{
    protected SignInManager SignInManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected IdentityLinkUserManager LinkUserManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    public AccountImpersonationAppService(
        SignInManager signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager securityLogManager,
        IdentityLinkUserManager linkUserManager,
        IOptions<IdentityOptions> identityOptions)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        SecurityLogManager = securityLogManager;
        LinkUserManager = linkUserManager;
        IdentityOptions = identityOptions;
    }

    [Authorize(IdentityPermissionNamesV2.Users.Impersonation)]
    public virtual async Task LoginAsync(Guid userId)
    {
        await IdentityOptions.SetAsync();

        if (userId == CurrentUser.Id)
        {
            throw new AbpAuthorizationException();
        }

        IdentityUser user = await UserManager.FindByIdAsync(userId.ToString()) ?? throw new UserNotFoundException();

        await ImpersonateLoginAsync(user);
    }

    public async Task LoginLoginAsync(Guid userId)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(userId);

        var source = new IdentityLinkUserInfo(CurrentUser.Id!.Value, CurrentUser.TenantId);
        var target = new IdentityLinkUserInfo(userId, user.TenantId);

        if (await LinkUserManager.IsLinkedAsync(source, target, true))
        {
            await SignInManager.SignInWithClaimsAsync(user, false, new Claim[0]);

            Logger.LogInformation("User with id '{0}' has been link login by user id '{1}'", user.Id, source.UserId);

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "ImpersonationLogin",
                UserName = user.UserName,
                ExtraProperties = { { "SourceUserId", source.UserId } }
            });
        }
        else
        {
            throw new BusinessException(AccountErrorCodes.UserNotLink);
        }
    }

    protected virtual async Task ImpersonateLoginAsync(IdentityUser user)
    {
        var currentUserId = CurrentUser.Id;

#pragma warning disable CS8604 // Possible null reference argument.
        IList<Claim> cliams = new List<Claim>() {
            new Claim(AbpClaimTypes.ImpersonatorUserId, CurrentUser.Id.ToString()),
            new Claim(AbpClaimTypes.ImpersonatorUserName, CurrentUser.UserName),
        };

        if (CurrentTenant.Id.HasValue)
        {
            cliams.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, CurrentTenant.Id.ToString()));
            cliams.Add(new Claim(AbpClaimTypes.ImpersonatorTenantName, CurrentTenant.Name));
        }
#pragma warning restore CS8604 // Possible null reference argument.

        await SignInManager.SignInWithClaimsAsync(user, false, cliams);

        Logger.LogInformation("User with id '{0}' has been impersonate login by user id '{1}'", user.Id, CurrentUser.Id);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "ImpersonationLogin",
            UserName = user.UserName,
            ExtraProperties = { { "impersonatorUserId", currentUserId.ToString() } }
        });
    }
}
