using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinitionUpdateDto
{
    public virtual bool IsEnabled { get; set; }
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
    public virtual string? Description { get; set; }
}
