using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Authorize]
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

    [Authorize(IdentityPermissionNamesV2.SecurityLogs.Default)]
    public virtual async Task<IdentitySecurityLogDto> GetAsync(Guid id)
    {
        var entity = await SecurityLogRepository.GetAsync(id);

        return ObjectMapper.Map<IdentitySecurityLog, IdentitySecurityLogDto>(entity);
    }

    [Authorize(IdentityPermissionNamesV2.SecurityLogs.Default)]
    public virtual async Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync(IdentitySecurityLogPagedListRequestDto input)
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

        return new PagedResultDto<IdentitySecurityLogDto>(count, ObjectMapper.Map<List<IdentitySecurityLog>, List<IdentitySecurityLogDto>>(list));
    }

    [Authorize]
    public virtual async Task<PagedResultDto<IdentitySecurityLogDto>> GetListByCurrentUserAsync(IdentitySecurityLogPagedListRequestDto input)
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
