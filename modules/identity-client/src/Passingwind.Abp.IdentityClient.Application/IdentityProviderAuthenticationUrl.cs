using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient;

public class IdentityProviderAuthenticationUrl : IIdentityProviderAuthenticationUrl, ISingletonDependency
{
    public string Get(IdentityClient client)
    {
        return $"/auth/external/identity/{client.Name}/login";
    }
}
