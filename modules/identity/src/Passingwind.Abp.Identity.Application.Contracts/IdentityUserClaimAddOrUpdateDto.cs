using System.Collections.Generic;

namespace Passingwind.Abp.Identity;

public class IdentityUserClaimAddOrUpdateDto
{
    public List<IdentityUserClaimDto> Items { get; set; } = null!;
}
