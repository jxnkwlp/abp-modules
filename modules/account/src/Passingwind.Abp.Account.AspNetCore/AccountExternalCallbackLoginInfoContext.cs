using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Passingwind.Abp.Account;

public class AccountExternalCallbackLoginInfoContext
{
    public ExternalLoginInfo LoginInfo { get; }

    public AccountExternalCallbackLoginInfoContext(ExternalLoginInfo loginInfo)
    {
        LoginInfo = loginInfo;
    }

    public bool Handled { get; set; }
    public IActionResult? Result { get; set; }
}
