namespace Passingwind.Abp.IdentityClient;

public class IdentityClientOpenIdConnectConfiguration
{
    public string Authority { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string? ClientSecret { get; set; }
    public string? MetadataAddress { get; set; }
    public bool? RequireHttpsMetadata { get; set; }
    public string? ResponseMode { get; set; }
    public string? ResponseType { get; set; }
    public bool? UsePkce { get; set; }
    public string? Scope { get; set; }
    public bool? GetClaimsFromUserInfoEndpoint { get; set; }
}
