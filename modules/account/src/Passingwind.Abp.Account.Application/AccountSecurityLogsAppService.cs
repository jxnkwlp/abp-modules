using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Account;

[Authorize]
public class AccountSecurityLogsAppService : AccountAppBaseService, IAccountSecurityLogsAppService
{
    protected IIdentitySecurityLogRepository SecurityLogRepository { get; }

    public AccountSecurityLogsAppService(IIdentitySecurityLogRepository securityLogRepository)
    {
        SecurityLogRepository = securityLogRepository;
    }

    public virtual async Task<PagedResultDto<IdentitySecurityLogsDto>> GetListAsync(AccountSecurityLogsPagedListRequestDto input)
    {
        var count = await SecurityLogRepository.GetCountAsync(
           startTime: input.StartTime,
           endTime: input.EndTime,
           applicationName: input.ApplicationName,
           identity: input.Identity,
           action: input.Action,
           userId: CurrentUser.Id,
           clientId: input.ClientId,
           correlationId: input.CorrelationId);

        var list = await SecurityLogRepository.GetListAsync(
            sorting: input.Sorting ?? nameof(IdentitySecurityLog.CreationTime) + " desc",
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userId: CurrentUser.Id,
            clientId: input.ClientId,
            correlationId: input.CorrelationId);

        return new PagedResultDto<IdentitySecurityLogsDto>(count, ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogsDto>>(list));
    }
}
