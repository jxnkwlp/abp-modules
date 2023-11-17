using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using IdentityUserManager = Passingwind.Abp.Identity.IdentityUserManager;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountLinkUserAppService : AccountAppBaseService, IAccountLinkUserAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityLinkUserManager LinkUserManager { get; }
    protected IdentitySecurityLogManager SecurityLogManager { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected ITenantStore TenantStore { get; }

    public AccountLinkUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IdentityLinkUserManager linkUserManager,
        IdentitySecurityLogManager securityLogManager,
        IOptions<IdentityOptions> identityOptions,
        ITenantStore tenantStore)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        LinkUserManager = linkUserManager;
        SecurityLogManager = securityLogManager;
        IdentityOptions = identityOptions;
        TenantStore = tenantStore;
    }

    public virtual async Task<ListResultDto<AccountLinkUserDto>> GetListAsync(AccountLinkUserListRequestDto input)
    {
        var list = await LinkUserManager.GetListAsync(new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentUser.TenantId), input.IncludeIndirect);

        var userIds = list.Select(x => x.TargetUserId).Concat(list.Select(x => x.SourceUserId)).Distinct();

        var allUsers = await UserRepository.GetListByIdsAsync(userIds);

        var results = new List<AccountLinkUserDto>();

        var tenantCache = new Dictionary<Guid, string?>();

        foreach (var item in list)
        {
            bool isDirectLink = item.TargetUserId == CurrentUser.GetId() && item.TargetTenantId == CurrentUser.TenantId;

            string? tenantName = string.Empty;
            if (item.SourceTenantId.HasValue)
            {
                if (tenantCache.ContainsKey(item.SourceTenantId.Value))
                {
                    tenantName = tenantCache[item.SourceTenantId.Value];
                }
                else
                {
                    tenantName = (await TenantStore.FindAsync(item.SourceTenantId.Value))?.Name;
                    tenantCache[item.SourceTenantId.Value] = tenantName;
                }
            }

            if (!results.Any(x => x.TargetUserId == item.SourceUserId && x.TargetTenantId == item.SourceTenantId))
            {
                results.Add(new AccountLinkUserDto()
                {
                    DirectlyLinked = isDirectLink,
                    TargetUserId = item.SourceUserId,
                    TargetUserName = allUsers.Find(u => u.Id == item.SourceUserId)?.UserName,
                    TargetTenantId = item.SourceTenantId,
                    TargetTenantName = tenantName,
                });
            }
        }

        return new ListResultDto<AccountLinkUserDto>(results);
    }

    public virtual async Task UnlinkAsync(AccountUnlinkDto input)
    {
        var source = new IdentityLinkUserInfo(input.UserId, input.TenantId);
        var target = new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentUser.TenantId);
        await LinkUserManager.UnlinkAsync(source, target);

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "Unlink",
            UserName = CurrentUser.UserName,
            ExtraProperties = { ["SourceUserId"] = input.UserId }
        });
    }

    public virtual async Task<AccountLinkDto> CreateLinkTokenAsync()
    {
        var target = new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentUser.TenantId);
        var token = await LinkUserManager.GenerateLinkTokenAsync(target, target.UserId.ToString());

        return new AccountLinkDto
        {
            Token = token,
            UserId = CurrentUser.GetId(),
        };
    }

    public virtual async Task<AccountLinkTokenValidationResultDto> VerifyLinkTokenAsync(AccountLinkTokenValidationRequestDto input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);

        var target = new IdentityLinkUserInfo(input.UserId, user.TenantId);
        var source = new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentUser.TenantId);

        var tokenVerified = await LinkUserManager.VerifyLinkTokenAsync(target, input.Token, target.UserId.ToString());

        if (!tokenVerified)
        {
            return new AccountLinkTokenValidationResultDto
            {
                Verified = false,
            };
        }

        if (!await LinkUserManager.IsLinkedAsync(source, target))
        {
            await LinkUserManager.LinkAsync(source, target);

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "LinkUser",
                UserName = CurrentUser.UserName,
                ExtraProperties = { ["TargetUserId"] = target.UserId }
            });
        }

        return new AccountLinkTokenValidationResultDto
        {
            Verified = true,
        };
    }
}
