using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountSecurityLogAppService : AccountAppBaseService, IAccountSecurityLogAppService
{
    protected IIdentitySecurityLogRepository SecurityLogRepository { get; }

    public AccountSecurityLogAppService(IIdentitySecurityLogRepository securityLogRepository)
    {
        SecurityLogRepository = securityLogRepository;
    }

    public virtual async Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input)
    {
        input.UserId = CurrentUser.Id;

        var count = await SecurityLogRepository.GetCountAsync(
           input.StartTime,
           input.EndTime,
           input.ApplicationName,
           input.Identity,
           input.Action,
           input.UserId,
           input.UserName,
           input.ClientId,
           input.CorrelationId);

        var list = await SecurityLogRepository.GetListAsync(
            input.Sorting ?? nameof(IdentitySecurityLog.CreationTime) + " desc",
            input.MaxResultCount,
            input.SkipCount,
            input.StartTime,
            input.EndTime,
            input.ApplicationName,
            input.Identity,
            input.Action,
            input.UserId,
            input.UserName,
            input.ClientId,
            input.CorrelationId);

        return new PagedResultDto<IdentitySecurityLogDto>(count, ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogDto>>(list));
    }
}
