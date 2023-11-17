using System;

namespace Passingwind.Abp.Account;

public class AccountUserDelegationCreateDto
{
    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
