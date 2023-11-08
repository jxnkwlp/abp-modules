using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using Passingwind.AspNetCore.Authentication.Saml2;
using Passingwind.AspNetCore.Authentication.Saml2.Configuration;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Saml2;

public class Saml2ConfigurationBuilder : ISaml2ConfigurationBuilder, ITransientDependency
{
    private readonly HttpClient _httpClient;

    public Saml2ConfigurationBuilder(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(Saml2ConfigurationBuilder));
    }

    public virtual async Task<Saml2Configuration> GetAsync(Saml2Options saml2Options, IdentityClientSaml2Configuration configuration, CancellationToken cancellationToken = default)
    {
        if (saml2Options.IdpMetadataUri == null)
            throw new System.ArgumentNullException(nameof(saml2Options.IdpMetadataUri));

        var manager = new ConfigurationManager(saml2Options, saml2Options.IdpMetadataUri!, _httpClient);

        return await manager.GetConfigurationAsync(cancellationToken);
    }
}
