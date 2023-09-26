using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountTfaVerifyTokenRequestDto
{
    [Required]
    public string Token { get; set; } = null!;
}
