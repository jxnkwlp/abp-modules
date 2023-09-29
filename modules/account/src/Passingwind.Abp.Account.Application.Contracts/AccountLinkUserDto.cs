using System;

namespace Passingwind.Abp.Identity;

public class AccountLinkUserDto
{
    public Guid TargetUserId { get; set; }
    public string? TargetUserName { get; set; }
    public Guid? TargetTenantId { get; set; }
    public string? TargetTenantName { get; set; }
    public bool DirectlyLinked { get; set; }
}
