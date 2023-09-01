using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitAddRoleRequestDto
{
    [Required]
    public Guid[] RoleIds { get; set; } = null!;
}
