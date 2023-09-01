using Microsoft.AspNetCore.Identity;

namespace Passingwind.Abp.Identity.AspNetCore;

public class AbpSignInResult : SignInResult
{
    public static AbpSignInResult ChangePasswordRequired { get; } = new AbpSignInResult { RequiresChangePassword = true };

    public bool RequiresChangePassword { get; private set; }

    public override string ToString()
    {
        return RequiresChangePassword ? "RequiresChangePassword" : base.ToString();
    }
}
