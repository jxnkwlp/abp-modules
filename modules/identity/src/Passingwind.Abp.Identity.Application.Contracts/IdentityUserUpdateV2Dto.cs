using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Passingwind.Abp.Identity;

public class IdentityUserUpdateV2Dto : IdentityUserCreateOrUpdateV2Dto, IHasConcurrencyStamp
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string? Password { get; set; }

    [Required]
    public string ConcurrencyStamp { get; set; } = null!;
}
