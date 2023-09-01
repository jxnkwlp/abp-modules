using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Passingwind.Abp.Account;

public class AccountRequiredChangePasswordRequestDto
{
    [Required]
    [StringLength(64)]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; } = null!;
}
