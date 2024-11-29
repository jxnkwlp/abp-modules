using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.AuditLogging.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;

namespace Passingwind.Abp.AuditLogging;

[Authorize(AuditLoggingPermissions.EntityChange.Default)]
public class EntityChangeAppService : AuditLoggingAppServiceBase, IEntityChangeAppService
{
    protected IAuditLogRepository AuditLogRepository { get; set; }

    public EntityChangeAppService(IAuditLogRepository auditLogRepository)
    {
        AuditLogRepository = auditLogRepository;
    }

    public virtual async Task<EntityChangeDto> GetAsync(Guid id)
    {
        var entity = await AuditLogRepository.GetEntityChange(id);

        return ObjectMapper.Map<EntityChange, EntityChangeDto>(entity);
    }

    public virtual async Task<ListResultDto<EntityChangeDto>> GetListAsync(EntityChangeListRequestDto input)
    {
        var list = await AuditLogRepository.GetEntityChangeListAsync(
            sorting: null,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            auditLogId: input.AuditLogId,
            startTime: input.StartTime,
            endTime: input.EndTime,
            changeType: input.ChangeType,
            entityId: input.EntityId,
            entityTypeFullName: input.EntityTypeFullName);

        var count = await AuditLogRepository.GetEntityChangeCountAsync(
            auditLogId: input.AuditLogId,
            startTime: input.StartTime,
            endTime: input.EndTime,
            changeType: input.ChangeType,
            entityId: input.EntityId,
            entityTypeFullName: input.EntityTypeFullName);

        return new PagedResultDto<EntityChangeDto>(count, ObjectMapper.Map<List<EntityChange>, List<EntityChangeDto>>(list));
    }
}
