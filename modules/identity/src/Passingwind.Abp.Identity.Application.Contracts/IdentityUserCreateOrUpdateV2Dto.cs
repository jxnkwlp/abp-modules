using System;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public abstract class IdentityUserCreateOrUpdateV2Dto : IdentityUserCreateOrUpdateDtoBase
{
    public virtual bool? ShouldChangePasswordOnNextLogin { get; set; }
    public virtual Guid[]? OrganizationUnitIds { get; set; }
}
