using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitAddUserRequestDto
{
    [Required]
    public Guid[] UserIds { get; set; } = null!;
}
