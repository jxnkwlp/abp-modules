using Volo.Abp.Account;

namespace Passingwind.Abp.Account;

public class AccountProfileDto : ProfileDto
{
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
}
