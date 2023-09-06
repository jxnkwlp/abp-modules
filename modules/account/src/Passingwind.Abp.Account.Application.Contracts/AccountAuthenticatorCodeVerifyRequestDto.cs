using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountAuthenticatorCodeVerifyRequestDto
{
    [Required]
    [MaxLength(8)]
    public string Code { get; set; } = null!;
}
