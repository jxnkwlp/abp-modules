using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Passingwind.Abp.Identity;

public class IdentityUserCreateV2Dto : IdentityUserCreateOrUpdateV2Dto
{
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string? Password { get; set; }
}
