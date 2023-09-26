using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountLoginVerifyTwoFactorTokenDto
{
    [Required]
    [MaxLength(16)]
    public string Token { get; set; } = null!;
}
