using System.Collections.Generic;

namespace Passingwind.Abp.Identity;

public class IdentityRoleClaimAddOrUpdateDto
{
    public List<IdentityRoleClaimDto> Items { get; set; } = null!;
}
