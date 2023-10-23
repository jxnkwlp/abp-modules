using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Passingwind.Abp.IdentityClient.OpenIdConnect;

public interface IOpenIdConnectOptionBuilder
{
    Task<OpenIdConnectOptions> GetAsync(string provider, IdentityClientOpenIdConnectConfiguration configuration, CancellationToken cancellationToken = default);
}
