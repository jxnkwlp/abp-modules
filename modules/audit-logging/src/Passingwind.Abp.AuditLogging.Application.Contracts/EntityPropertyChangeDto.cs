using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.AuditLogging;

public class EntityPropertyChangeDto : EntityDto<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; set; }

    public virtual Guid EntityChangeId { get; set; }

    public virtual string? NewValue { get; set; }

    public virtual string? OriginalValue { get; set; }

    public virtual string? PropertyName { get; set; }

    public virtual string? PropertyTypeFullName { get; set; }
}
