using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class ExternalLoginInfoReceivedContext : ExternalLoginProviderBaseContext
{
    public ExternalLoginInfoReceivedContext(HttpContext httpContext) : base(httpContext)
    {
    }

    public ExternalLoginInfo ExternalLoginInfo { get; set; } = null!;
}
