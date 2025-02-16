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
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userId: input.UserId,
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId,
            clientIpAddress: input.ClientIpAddress);

        var list = await SecurityLogRepository.GetListAsync(
            sorting: input.Sorting ?? nameof(IdentitySecurityLog.CreationTime) + " desc",
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            applicationName: input.ApplicationName,
            identity: input.Identity,
            action: input.Action,
            userId: input.UserId,
            userName: input.UserName,
            clientId: input.ClientId,
            correlationId: input.CorrelationId,
            clientIpAddress: input.ClientIpAddress);

        return new PagedResultDto<IdentitySecurityLogsDto>(count, ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogsDto>>(list));
    }
}
