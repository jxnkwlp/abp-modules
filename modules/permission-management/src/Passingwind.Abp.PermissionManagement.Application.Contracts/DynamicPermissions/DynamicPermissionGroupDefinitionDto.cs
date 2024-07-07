using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionGroupDefinitionDto : AuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string DisplayName { get; set; } = null!;
}
