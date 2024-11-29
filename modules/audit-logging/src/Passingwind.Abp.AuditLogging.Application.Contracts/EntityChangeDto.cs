using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.AuditLogging;

public class EntityChangeDto : EntityDto<Guid>, IMultiTenant, IHasExtraProperties
{
    public virtual Guid AuditLogId { get; set; }

    public virtual Guid? TenantId { get; set; }

    public virtual DateTime ChangeTime { get; set; }

    public virtual EntityChangeType ChangeType { get; set; }

    public virtual Guid? EntityTenantId { get; set; }

    public virtual string? EntityId { get; set; }

    public virtual string? EntityTypeFullName { get; set; }

    public virtual List<EntityPropertyChangeDto>? PropertyChanges { get; set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; set; } = null!;
}
