using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountProfileChangeEmailDto : AccountVerifyTokenRequestDto
{
    [EmailAddress]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;
}
