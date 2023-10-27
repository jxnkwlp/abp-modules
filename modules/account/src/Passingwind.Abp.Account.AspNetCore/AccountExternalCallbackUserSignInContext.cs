using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.Account;

public class AccountExternalCallbackUserSignInContext
{
    public ExternalLoginInfo LoginInfo { get; }

    public AccountExternalCallbackUserSignInContext(ExternalLoginInfo loginInfo, IdentityUser? user)
    {
        LoginInfo = loginInfo;
        User = user;
    }

    public IdentityUser? User { get; set; }

    public bool Handled { get; set; }

    public IActionResult? Result { get; set; }
}
