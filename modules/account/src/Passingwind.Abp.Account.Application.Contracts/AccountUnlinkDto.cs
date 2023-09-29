using System;

namespace Passingwind.Abp.Identity;

public class AccountUnlinkDto
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
}
