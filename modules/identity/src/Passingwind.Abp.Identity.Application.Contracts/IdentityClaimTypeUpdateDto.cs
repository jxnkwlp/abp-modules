using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Passingwind.Abp.Identity;

public class IdentityClaimTypeUpdateDto : ExtensibleEntityDto
{
    [Required]
    [DynamicMaxLength(typeof(IdentityClaimTypeConsts), nameof(IdentityClaimTypeConsts.MaxNameLength))]
    public virtual string Name { get; set; } = null!;
    public virtual bool Required { get; set; }
    [DynamicMaxLength(typeof(IdentityClaimTypeConsts), nameof(IdentityClaimTypeConsts.MaxRegexLength))]
    public virtual string? Regex { get; set; }
    [DynamicMaxLength(typeof(IdentityClaimTypeConsts), nameof(IdentityClaimTypeConsts.MaxRegexDescriptionLength))]
    public virtual string? RegexDescription { get; set; }
    [DynamicMaxLength(typeof(IdentityClaimTypeConsts), nameof(IdentityClaimTypeConsts.MaxDescriptionLength))]
    public virtual string? Description { get; set; }
    public virtual IdentityClaimValueType ValueType { get; set; }
}
