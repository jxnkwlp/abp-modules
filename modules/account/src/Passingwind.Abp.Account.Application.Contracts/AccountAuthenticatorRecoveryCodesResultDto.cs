using System.Collections.Generic;

namespace Passingwind.Abp.Account;

public class AccountAuthenticatorRecoveryCodesResultDto
{
    public IEnumerable<string> RecoveryCodes { get; set; } = null!;
}
