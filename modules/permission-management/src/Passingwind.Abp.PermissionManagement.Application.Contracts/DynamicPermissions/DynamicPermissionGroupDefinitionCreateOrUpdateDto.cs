﻿using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionGroupDefinitionCreateOrUpdateDto
{
    [Required]
    [MaxLength(64)]
    public virtual string Name { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
}
