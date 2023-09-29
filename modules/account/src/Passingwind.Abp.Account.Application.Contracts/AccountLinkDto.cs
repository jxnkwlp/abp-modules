using System;

namespace Passingwind.Abp.Identity;

public class AccountLinkDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
}
