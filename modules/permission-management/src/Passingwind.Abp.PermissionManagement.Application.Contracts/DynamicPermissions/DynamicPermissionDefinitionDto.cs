using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinitionDto : AuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string DisplayName { get; set; } = null!;
    public virtual bool IsEnabled { get; set; }
    public virtual Guid? GroupId { get; set; }
    public virtual Guid? ParentId { get; set; }
    public virtual string? Description { get; set; }
    // public virtual IEnumerable<string> PermissionLinks { get; set; } = null!;
}
