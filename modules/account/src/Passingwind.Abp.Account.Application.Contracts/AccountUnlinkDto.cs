using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class AccountUnlinkDto
{
    [Required]
    public Guid UserId { get; set; }
    public Guid? TenantId { get; set; }
}
