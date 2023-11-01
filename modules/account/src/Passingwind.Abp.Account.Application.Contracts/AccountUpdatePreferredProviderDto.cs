using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountUpdatePreferredProviderDto
{
    [Required]
    [MaxLength(64)]
    public string Provider { get; set; } = null!;
}
