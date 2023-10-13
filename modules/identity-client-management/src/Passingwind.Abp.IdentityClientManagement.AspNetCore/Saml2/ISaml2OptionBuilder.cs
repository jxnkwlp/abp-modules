using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Passingwind.AspNetCore.Authentication.Saml2;

namespace Passingwind.Abp.IdentityClientManagement.Saml2;

public interface ISaml2OptionBuilder
{
    Task<Saml2Options> GetAsync(string provider, IdentityClientSaml2Configuration configuration, CancellationToken cancellationToken = default);
}
