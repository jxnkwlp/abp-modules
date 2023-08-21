namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientOpenIdConnectConfigurationDto
{
    public string? Authority { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? MetadataAddress { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string? ResponseMode { get; set; }
    public string? ResponseType { get; set; }
    public bool UsePkce { get; set; }
    public string? Scope { get; set; }
    public bool? GetClaimsFromUserInfoEndpoint { get; set; }
}
