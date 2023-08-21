using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class ExternalLoginRedirectToIdentityProviderContext : ExternalLoginProviderBaseContext
{
    public ExternalLoginRedirectToIdentityProviderContext(HttpContext httpContext) : base(httpContext)
    {
    }

    public AuthenticationProperties AuthenticationProperties { get; set; } = null!;
}
