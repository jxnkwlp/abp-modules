using System.Collections.Generic;

namespace Passingwind.Abp.Account;

public class AccountTfaDto
{
    public bool Enabled { get; set; }

    public bool IsMachineRemembered { get; set; }

    public IEnumerable<string> Providers { get; set; } = null!;
}
