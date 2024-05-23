using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissionNamesV2.SecurityLogs.Default)]
public class IdentitySecurityLogAppService : IdentityAppBaseService, IIdentitySecurityLogAppService
{
    protected IIdentitySecurityLogRepository SecurityLogRepository { get; }

    public IdentitySecurityLogAppService(IIdentitySecurityLogRepository securityLogRepository)
    {
        SecurityLogRepository = securityLogRepository;
    }

    [Authorize(IdentityPermissionNamesV2.SecurityLogs.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await SecurityLogRepository.DeleteAsync(id);
    }

    public virtual async Task<IdentitySecurityLogsDto> GetAsync(Guid id)
    {
        var entity = await SecurityLogRepository.GetAsync(id);

        return ObjectMapper.Map<IdentitySecurityLog, IdentitySecurityLogsDto>(entity);
    }

    public virtual async Task<PagedResultDto<IdentitySecurityLogsDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input)
    {
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

        return new PagedResultDto<IdentitySecurityLogsDto>(count, ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogsDto>>(list));
    }
}
