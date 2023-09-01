using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitCreateDto
{
    public virtual Guid? ParentId { get; set; }

    [Required]
    [DynamicMaxLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxDisplayNameLength))]
    public virtual string DisplayName { get; set; } = null!;
}

public class OrganizationUnitUpdateDto
{
    [Required]
    [DynamicMaxLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxDisplayNameLength))]
    public virtual string DisplayName { get; set; } = null!;
}
