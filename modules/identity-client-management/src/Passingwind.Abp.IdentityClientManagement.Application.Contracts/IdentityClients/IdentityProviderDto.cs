namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityProviderDto
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public IdentityProviderType ProviderType { get; set; }
    public string AuthenticationUrl { get; set; } = null!;
}
