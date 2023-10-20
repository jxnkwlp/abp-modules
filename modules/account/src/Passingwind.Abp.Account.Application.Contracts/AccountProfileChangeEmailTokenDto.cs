using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountProfileChangeEmailTokenDto
{
    [EmailAddress]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;
}
