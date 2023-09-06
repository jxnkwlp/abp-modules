using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountVerifyTokenRequestDto
{
    [Required]
    [MaxLength(32)]
    public string Provider { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;
}
