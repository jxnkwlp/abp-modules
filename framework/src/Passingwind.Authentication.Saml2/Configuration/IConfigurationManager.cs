using System.Threading;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;

namespace Passingwind.Authentication.Saml2.Configuration;

public interface IConfigurationManager
{
    Task<Saml2Configuration> GetConfigurationAsync(CancellationToken cancellationToken = default);
}
