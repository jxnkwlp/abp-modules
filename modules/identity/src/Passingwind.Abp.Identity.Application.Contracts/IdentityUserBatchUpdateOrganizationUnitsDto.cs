using System;

namespace Passingwind.Abp.Identity;

public class IdentityUserBatchUpdateOrganizationUnitsDto : IdentityUserBatchInputDto
{
    public bool Override { get; set; }
    public Guid[]? OrganizationUnitIds { get; set; }
}
