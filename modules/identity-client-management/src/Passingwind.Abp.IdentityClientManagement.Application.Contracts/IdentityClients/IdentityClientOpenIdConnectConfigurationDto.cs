using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientOpenIdConnectConfigurationDto
{
    [Required]
    [MaxLength(256)]
    public string Authority { get; set; } = null!;
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    [Required]
    [MaxLength(256)]
    public string? MetadataAddress { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string? ResponseMode { get; set; }
    public string? ResponseType { get; set; }
    public bool UsePkce { get; set; }
    public string? Scope { get; set; }
    public bool? GetClaimsFromUserInfoEndpoint { get; set; }
}
