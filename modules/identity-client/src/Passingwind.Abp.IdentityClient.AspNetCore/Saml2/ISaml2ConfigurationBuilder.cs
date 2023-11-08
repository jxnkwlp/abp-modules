using System.Threading;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using Passingwind.AspNetCore.Authentication.Saml2;

namespace Passingwind.Abp.IdentityClient.Saml2;

public interface ISaml2ConfigurationBuilder
{
    Task<Saml2Configuration> GetAsync(Saml2Options saml2Options, IdentityClientSaml2Configuration configuration, CancellationToken cancellationToken = default);
}
