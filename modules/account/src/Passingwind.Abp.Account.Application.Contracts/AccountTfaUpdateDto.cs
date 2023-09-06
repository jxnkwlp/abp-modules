using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountTfaUpdateDto
{
    [Required]
    [MaxLength(8)]
    public string Code { get; set; } = null!;
}
