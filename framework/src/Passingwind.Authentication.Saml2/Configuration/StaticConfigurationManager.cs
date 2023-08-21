using System.Threading;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;

namespace Passingwind.Authentication.Saml2.Configuration;

public class StaticConfigurationManager : IConfigurationManager
{
    private readonly Saml2Configuration _saml2Configuration;

    public StaticConfigurationManager(Saml2Configuration saml2Configuration)
    {
        _saml2Configuration = saml2Configuration;
    }

    public Task<Saml2Configuration> GetConfigurationAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_saml2Configuration);
    }
}
