using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionDefinitionUpdateDto
{
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
    public virtual string? Description { get; set; }
}
