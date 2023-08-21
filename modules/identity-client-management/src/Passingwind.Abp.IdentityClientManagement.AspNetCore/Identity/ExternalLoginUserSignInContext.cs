using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class ExternalLoginUserSignInContext : ExternalLoginProviderBaseContext
{
    public ExternalLoginUserSignInContext(HttpContext httpContext, ExternalLoginInfo externalLoginInfo) : base(httpContext)
    {
        ExternalLoginInfo = externalLoginInfo;
    }

    public ExternalLoginInfo ExternalLoginInfo { get; }

    public IdentityUser IdentityUser { get; set; } = null!;
}
