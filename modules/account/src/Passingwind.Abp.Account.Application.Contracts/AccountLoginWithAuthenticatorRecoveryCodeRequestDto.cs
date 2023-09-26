using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountLoginWithAuthenticatorRecoveryCodeRequestDto
{
    [Required]
    [MaxLength(256)]
    public string RecoveryCode { get; set; } = null!;
}
