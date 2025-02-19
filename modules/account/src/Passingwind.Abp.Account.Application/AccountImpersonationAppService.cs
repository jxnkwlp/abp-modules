﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Account.Events;
using Passingwind.Abp.Identity;
using Passingwind.Abp.Identity.AspNetCore;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
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
    protected IdentityUserDelegationManager UserDelegationManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected ILocalEventBus LocalEventBus { get; }

    public AccountImpersonationAppService(
        SignInManager signInManager,
        IdentityUserManager userManager,
        IdentitySecurityLogManager securityLogManager,
        IdentityLinkUserManager linkUserManager,
        IOptions<IdentityOptions> identityOptions,
        IdentityUserDelegationManager userDelegationManager,
        ILocalEventBus localEventBus)
    {
        SignInManager = signInManager;
        UserManager = userManager;
        SecurityLogManager = securityLogManager;
        LinkUserManager = linkUserManager;
        IdentityOptions = identityOptions;
        UserDelegationManager = userDelegationManager;
        LocalEventBus = localEventBus;
    }

    public virtual async Task LogoutAsync()
    {
        await IdentityOptions.SetAsync();

        var userId = CurrentUser.FindImpersonatorUserId();
        var tenantId = CurrentUser.FindImpersonatorTenantId();

        if (!userId.HasValue)
        {
            throw new AbpAuthorizationException();
        }

        if (tenantId.HasValue)
        {
            CurrentTenant.Change(tenantId.Value);
        }

        await IdentityOptions.SetAsync();

        var user = await UserManager.FindByIdAsync(userId.Value.ToString()) ?? throw new UserNotFoundException();

        await SignInManager.SignInAsync(user, false);
    }

    [Authorize(IdentityPermissionNamesV2.Users.Impersonation)]
    public virtual async Task LoginAsync(Guid userId)
    {
        await IdentityOptions.SetAsync();

        if (userId == CurrentUser.Id)
        {
            throw new AbpAuthorizationException();
        }

        var currentUserId = CurrentUser.Id;
        var currentUserName = CurrentUser.Name;

        IdentityUser user = await UserManager.FindByIdAsync(userId.ToString()) ?? throw new UserNotFoundException();

        await ImpersonateLoginAsync(user);

        var loginEventData = new Dictionary<string, object>
            {
                { "CurrentUserName", currentUserName! },
                { "CurrentUserId", currentUserId! },
            };

        await LocalEventBus.PublishAsync(new UserLoginEvent(user.Id, UserLoginEvent.ImpersonationLogout, loginEventData), onUnitOfWorkComplete: true);
    }

    public virtual async Task LinkLoginAsync(Guid userId)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(userId);

        var source = new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentUser.TenantId);
        var target = new IdentityLinkUserInfo(userId, user.TenantId);

        var currentUserId = CurrentUser.Id;
        var currentUserName = CurrentUser.Name;

        if (user.TenantId.HasValue && user.TenantId != CurrentUser.TenantId)
        {
            CurrentTenant.Change(user.TenantId.Value);
        }

        if (await LinkUserManager.IsLinkedAsync(source, target, true))
        {
            await ImpersonateLoginAsync(user);

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "LinkLogin",
                UserName = user.UserName,
                ExtraProperties = { { "SourceUserId", source.UserId } }
            });

            var loginEventData = new Dictionary<string, object>
            {
                { "CurrentUserName", currentUserName! },
                { "CurrentUserId", currentUserId! },
            };

            await LocalEventBus.PublishAsync(new UserLoginEvent(user.Id, UserLoginEvent.LinkLogin, loginEventData), onUnitOfWorkComplete: true);
        }
        else
        {
            throw new BusinessException(AccountErrorCodes.UserNotLink);
        }
    }

    public virtual async Task DelegationLoginAsync(Guid id)
    {
        var delegation = await UserDelegationManager.FindActiveDelegationByIdAsync(id);

        // check the delegated is for me.
        if (delegation == null || delegation.TargetUserId != CurrentUser.GetId())
        {
            throw new BusinessException(AccountErrorCodes.UserNotDelegated);
        }

        var currentUserId = CurrentUser.Id;
        var currentUserName = CurrentUser.Name;

        // Get the delegation source user 
        var user = await UserManager.GetByIdAsync(delegation.SourceUserId);

        await ImpersonateLoginAsync(user);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "DelegationLogin",
            UserName = user.UserName,
            ExtraProperties = { { "SourceUserId", CurrentUser.GetId() } }
        });

        var loginEventData = new Dictionary<string, object>
            {
                { "CurrentUserName", currentUserName! },
                { "CurrentUserId", currentUserId! },
            };

        await LocalEventBus.PublishAsync(new UserLoginEvent(user.Id, UserLoginEvent.DelegationLogin, loginEventData), onUnitOfWorkComplete: true);
    }

    protected virtual async Task ImpersonateLoginAsync(IdentityUser user)
    {
        if (CurrentUser?.IsAuthenticated != true)
            throw new AbpAuthorizationException();

        IList<Claim> cliams = new List<Claim>() {
            new Claim(AbpClaimTypes.ImpersonatorUserId, CurrentUser.GetId().ToString()),
            new Claim(AbpClaimTypes.ImpersonatorUserName, CurrentUser.UserName!),
        };

        if (CurrentTenant.Id.HasValue)
        {
            cliams.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, CurrentTenant.GetId().ToString()));
            cliams.Add(new Claim(AbpClaimTypes.ImpersonatorTenantName, CurrentTenant.Name!));
        }

        await SignInManager.SignInWithClaimsAsync(user, false, cliams);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "ImpersonationLogin",
            UserName = user.UserName,
            ExtraProperties = { { "impersonatorUserId", CurrentUser.GetId().ToString() } }
        });
    }
}
