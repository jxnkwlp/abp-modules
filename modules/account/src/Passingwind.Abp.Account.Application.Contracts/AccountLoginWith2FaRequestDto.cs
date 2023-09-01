using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Account;

public class AccountLoginWith2FaRequestDto
{
    [Required]
    [MaxLength(16)]
    public string Code { get; set; } = null!;

    public bool RememberMe { get; set; }

    public bool RememberMachine { get; set; }
}
