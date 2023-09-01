using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class IdentityClaimDto : EntityDto
{
    /// <summary>
    /// Gets or sets the claim type for this claim.
    /// </summary>
    public virtual string ClaimType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the claim value for this claim.
    /// </summary>
    public virtual string ClaimValue { get; set; } = null!;
}
