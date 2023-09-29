using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
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

    public AccountLinkUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IdentityLinkUserManager linkUserManager,
        IdentitySecurityLogManager securityLogManager,
        IOptions<IdentityOptions> identityOptions)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        LinkUserManager = linkUserManager;
        SecurityLogManager = securityLogManager;
        IdentityOptions = identityOptions;
    }

    public virtual async Task<ListResultDto<AccountLinkUserDto>> GetListAsync(AccountLinkUserListRequestDto input)
    {
        var list = await LinkUserManager.GetListAsync(new IdentityLinkUserInfo(CurrentUser.Id!.Value, CurrentUser.TenantId), input.IncludeIndirect);

        var userIds = list.Select(x => x.TargetUserId).Distinct();

        var users = await UserRepository.GetListByIdsAsync(userIds);

        return new ListResultDto<AccountLinkUserDto>(list.ConvertAll(x => new AccountLinkUserDto()
        {
            DirectlyLinked = x.SourceUserId == CurrentUser.Id || x.TargetUserId == CurrentUser.Id,
            TargetUserId = x.TargetUserId,
            TargetUserName = users.Find(u => u.Id == x.TargetUserId)?.Name,
            TargetTenantId = x.TargetTenantId,
            TargetTenantName = "" // TODO
        }));
    }

    public virtual async Task UnlinkAsync(AccountUnlinkDto input)
    {
        await LinkUserManager.UnlinkAsync(new IdentityLinkUserInfo(CurrentUser.Id!.Value, CurrentUser.TenantId), new IdentityLinkUserInfo(input.UserId, input.TenantId));

        await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "Unlink",
            UserName = CurrentUser.UserName,
            ExtraProperties = { ["TargetUserId"] = input.UserId }
        });
    }

    public virtual async Task<AccountLinkDto> CreateLinkTokenAsync()
    {
        var source = new IdentityLinkUserInfo(CurrentUser.Id!.Value, CurrentUser.TenantId);
        var token = await LinkUserManager.GenerateLinkTokenAsync(source, source.UserId.ToString());

        return new AccountLinkDto
        {
            Token = token,
            UserId = CurrentUser.Id!.Value,
        };
    }

    public virtual async Task<AccountLinkTokenValidationResultDto> VerifyLinkTokenAsync(AccountLinkTokenValidationRequestDto input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);

        var source = new IdentityLinkUserInfo(input.UserId, user.TenantId);
        var target = new IdentityLinkUserInfo(CurrentUser.Id!.Value, CurrentUser.TenantId);

        var tokenVerified = await LinkUserManager.VerifyLinkTokenAsync(source, input.Token, source.UserId.ToString());

        if (tokenVerified)
        {
            if (!await LinkUserManager.IsLinkedAsync(source, target))
            {
                await LinkUserManager.LinkAsync(source, target);
            }

            await SecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "LinkUser",
                UserName = CurrentUser.UserName,
                ExtraProperties = { ["SourceUserId"] = source.UserId }
            });

            return new AccountLinkTokenValidationResultDto
            {
                Verified = true,
            };
        }

        return new AccountLinkTokenValidationResultDto
        {
            Verified = false,
        };
    }
}
