using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinitionCreateDto : DynamicPermissionDefinitionUpdateDto
{
    [Required]
    [MaxLength(64)]
    public virtual string Name { get; set; } = null!;
    public virtual Guid GroupId { get; set; }
    public virtual Guid? ParentId { get; set; }
}
