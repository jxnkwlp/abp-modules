using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class IdentityUserUpdateOrganizationUnitsDto
{
    [Required]
    public Guid[] Ids { get; set; } = null!;
}
