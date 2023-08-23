using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionGroupDefinitionCreateOrUpdateDto
{
    [Required]
    [MaxLength(64)]
    public virtual string Name { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
}
