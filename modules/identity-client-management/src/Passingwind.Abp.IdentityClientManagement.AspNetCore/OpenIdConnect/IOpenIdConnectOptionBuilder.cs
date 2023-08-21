using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement.OpenIdConnect;

public interface IOpenIdConnectOptionBuilder
{
    Task<OpenIdConnectOptions> GetAsync(string provider, IdentityClientOpenIdConnectConfiguration configuration, CancellationToken cancellationToken = default);
}
