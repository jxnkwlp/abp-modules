using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.AuditLogging.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;

namespace Passingwind.Abp.AuditLogging;

[Authorize(AuditLoggingPermissions.AuditLogs.Default)]
public class AuditLogAppService : AuditLoggingAppServiceBase, IAuditLogAppService
{
    protected IAuditLogRepository AuditLogRepository { get; }

    public AuditLogAppService(IAuditLogRepository auditLogRepository)
    {
        AuditLogRepository = auditLogRepository;
    }

    [Authorize(AuditLoggingPermissions.AuditLogs.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await AuditLogRepository.DeleteAsync(id);
    }

    public async Task<AuditLogDto> GetAsync(Guid id)
    {
        var entity = await AuditLogRepository.GetAsync(id);

        return ObjectMapper.Map<AuditLog, AuditLogDto>(entity);
    }

    public async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogListRequestDto input)
    {
        var list = await AuditLogRepository.GetListAsync(
            sorting: input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            httpMethod: input.HttpMethod,
            url: input.Url,
            userId: input.UserId,
            userName: input.UserName,
            applicationName: input.ApplicationName,
            clientIpAddress: input.ClientIpAddress,
            correlationId: input.CorrelationId,
            maxExecutionDuration: input.MaxExecutionDuration,
            minExecutionDuration: input.MinExecutionDuration,
            hasException: input.HasException,
            httpStatusCode: (System.Net.HttpStatusCode?)input.HttpStatusCode);

        var count = await AuditLogRepository.GetCountAsync(
            startTime: input.StartTime,
            endTime: input.EndTime,
            httpMethod: input.HttpMethod,
            url: input.Url,
            userId: input.UserId,
            userName: input.UserName,
            applicationName: input.ApplicationName,
            clientIpAddress: input.ClientIpAddress,
            correlationId: input.CorrelationId,
            maxExecutionDuration: input.MaxExecutionDuration,
            minExecutionDuration: input.MinExecutionDuration,
            hasException: input.HasException,
            httpStatusCode: (System.Net.HttpStatusCode?)input.HttpStatusCode);

        return new PagedResultDto<AuditLogDto>(count, ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(list));
    }
}
