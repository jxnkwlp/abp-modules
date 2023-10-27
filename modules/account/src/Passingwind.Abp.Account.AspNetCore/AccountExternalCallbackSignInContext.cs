using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Passingwind.Abp.Account;

public class AccountExternalCallbackSignInContext
{
    public SignInResult SignInResult { get; }

    public AccountExternalCallbackSignInContext(SignInResult signInResult)
    {
        SignInResult = signInResult;
    }

    public bool Handled { get; set; }
    public IActionResult? Result { get; set; }
}
