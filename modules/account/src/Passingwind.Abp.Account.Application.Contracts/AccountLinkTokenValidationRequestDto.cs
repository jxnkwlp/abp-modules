using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class AccountLinkTokenValidationRequestDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; } = null!;
}
