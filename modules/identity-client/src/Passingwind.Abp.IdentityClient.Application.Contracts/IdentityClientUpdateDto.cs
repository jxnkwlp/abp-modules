using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientUpdateDto : ExtensibleObject
{
    [MaxLength(64)]
    [Required]
    public virtual string DisplayName { get; set; } = null!;
    public virtual IdentityProviderType ProviderType { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual int DisplayOrder { get; set; }
    public virtual bool IsDebugMode { get; set; }

    public virtual List<IdentityClientClaimMapDto>? ClaimMaps { get; set; }
    public virtual List<string>? RequiredClaimTypes { get; set; }

    public virtual IdentityClientOpenIdConnectConfigurationDto? OpenIdConnectConfiguration { get; set; }
    public virtual IdentityClientSaml2ConfigurationDto? Saml2Configuration { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }

        if (ProviderType == IdentityProviderType.OpenIdConnect && OpenIdConnectConfiguration == null)
        {
            yield return new ValidationResult("OpenIdConnectConfiguration should not be null", new[] { nameof(OpenIdConnectConfiguration) });
        }
        else if (ProviderType == IdentityProviderType.Saml2 && Saml2Configuration == null)
        {
            yield return new ValidationResult("Saml2Configuration should not be null", new[] { nameof(Saml2Configuration) });
        }
    }
}
