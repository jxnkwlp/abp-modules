using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class Account2FaCodeSendDto
{
    [Required]
    [MaxLength(32)]
    public string Provider { get; set; } = null!;
}
