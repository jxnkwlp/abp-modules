using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionDefinitionCreateDto
{
    [Required]
    [MaxLength(64)]
    public virtual string Name { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
    public virtual Guid GroupId { get; set; }
    public virtual Guid? ParentId { get; set; }
    public virtual string? Description { get; set; }
}
