using System;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public abstract class IdentityUserCreateOrUpdateV2Dto : IdentityUserCreateOrUpdateDtoBase
{
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public virtual bool? ShouldChangePasswordOnNextLogin { get; set; }
    public virtual Guid[]? OrganizationUnitIds { get; set; }
}
