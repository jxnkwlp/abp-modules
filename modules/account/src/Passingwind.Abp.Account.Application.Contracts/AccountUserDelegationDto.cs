using System;

namespace Passingwind.Abp.Account;

public class AccountUserDelegationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }
}
