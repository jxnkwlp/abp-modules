namespace Passingwind.Abp.IdentityClient;

public class IdentityClientSaml2Configuration
{
    public string? IdpMetadataUrl { get; set; }
    public string? IdpMetadataContent { get; set; }

    public string? Issuer { get; set; }

    public bool? TrustCertificate { get; set; }

    public bool? ForceAuthn { get; set; }
    public bool? RequireAssertionsSigned { get; set; }

    public string? SigningCertificatePem { get; set; }
    public string? SigningCertificateKeyPem { get; set; }

    public bool? UseGetAsAssertionConsumerService { get; set; }
    public bool? UseGetAsSingleLogoutService { get; set; }

    public string? CallbackPath { get; set; }
    public string? SignOutPath { get; set; }

    public string? NameIDFormats { get; set; }
}
