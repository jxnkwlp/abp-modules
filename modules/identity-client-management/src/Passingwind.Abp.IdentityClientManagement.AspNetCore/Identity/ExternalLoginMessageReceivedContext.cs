using Microsoft.AspNetCore.Http;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class ExternalLoginMessageReceivedContext : ExternalLoginProviderBaseContext
{
    public ExternalLoginMessageReceivedContext(HttpContext httpContext) : base(httpContext)
    {
    }

    public string? RemoteError { get; set; }
}
