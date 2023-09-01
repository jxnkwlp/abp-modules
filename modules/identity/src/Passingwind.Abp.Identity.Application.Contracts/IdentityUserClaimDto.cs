using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Passingwind.Abp.Identity;

public class IdentityUserClaimDto
{
    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(IdentityRoleClaimConsts), nameof(IdentityUserClaimConsts.MaxClaimTypeLength))]
    public virtual string ClaimType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(IdentityRoleClaimConsts), nameof(IdentityUserClaimConsts.MaxClaimValueLength))]
    public virtual string ClaimValue { get; set; } = null!;
}
