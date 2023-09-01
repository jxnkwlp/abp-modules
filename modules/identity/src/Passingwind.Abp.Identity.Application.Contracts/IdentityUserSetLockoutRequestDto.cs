using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class IdentityUserSetLockoutRequestDto
{
    [Required]
    public DateTime EndTime { get; set; }
}
