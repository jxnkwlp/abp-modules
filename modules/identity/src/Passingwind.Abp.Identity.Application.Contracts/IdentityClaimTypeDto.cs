using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public class IdentityClaimTypeDto : ExtensibleEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual bool Required { get; set; }
    public virtual bool IsStatic { get; set; }
    public virtual string? Regex { get; set; }
    public virtual string? RegexDescription { get; set; }
    public virtual string? Description { get; set; }
    public virtual IdentityClaimValueType ValueType { get; set; }
    public virtual string ValueTypeAsString => ValueType.ToString();
}
