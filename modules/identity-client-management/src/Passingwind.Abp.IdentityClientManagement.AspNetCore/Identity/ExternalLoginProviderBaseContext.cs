using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class ExternalLoginProviderBaseContext
{
    public HttpContext HttpContext { get; }

    public bool Handled { get; private set; }

    public IActionResult? Result { get; set; }

    public virtual string? ReturnUrl { get; set; }

    public ExternalLoginProviderBaseContext(HttpContext httpContext)
    {
        HttpContext = httpContext;
    }

    public void HandleResponse()
    {
        Handled = true;
    }
}
