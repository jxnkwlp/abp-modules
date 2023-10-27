namespace Passingwind.Abp.IdentityClient;

public interface IIdentityProviderAuthenticationUrl
{
    string Get(IdentityClient client);
}
