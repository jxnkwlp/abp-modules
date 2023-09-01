using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Passingwind.Abp.Account;

public class AccountLoginRequestDto
{
    [Required]
    [StringLength(255)]
    public string UserNameOrEmailAddress { get; set; } = null!;

    [Required]
    [StringLength(64)]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
