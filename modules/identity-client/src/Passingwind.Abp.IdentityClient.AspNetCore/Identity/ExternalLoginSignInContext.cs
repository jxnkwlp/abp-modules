using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Passingwind.Abp.IdentityClient.Identity;

public class ExternalLoginSignInContext : ExternalLoginProviderBaseContext
{
    public ExternalLoginSignInContext(HttpContext httpContext, ExternalLoginInfo externalLoginInfo) : base(httpContext)
    {
        ExternalLoginInfo = externalLoginInfo;
    }

    public ExternalLoginInfo ExternalLoginInfo { get; }

    public SignInResult SignInResult { get; set; } = null!;
}
