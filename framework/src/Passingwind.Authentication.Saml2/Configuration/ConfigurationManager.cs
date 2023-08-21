using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;

namespace Passingwind.Authentication.Saml2.Configuration;

public class ConfigurationManager : IConfigurationManager
{
    private Saml2Configuration? _saml2Configuration;

    private readonly Saml2Options _options;
    private readonly Uri _idpMetadataUri;
    private readonly HttpClient _httpClient;

    public ConfigurationManager(Saml2Options options, Uri idpMetadataUrl, HttpClient httpClient)
    {
        _options = options;
        _idpMetadataUri = idpMetadataUrl;
        _httpClient = httpClient;
    }

    public async Task<Saml2Configuration> GetConfigurationAsync(CancellationToken cancellationToken = default)
    {
        if (_saml2Configuration != null)
            return _saml2Configuration;

        var configuration = new Saml2Configuration()
        {
            Issuer = _options.Issuer,
            CertificateValidationMode = _options.CertificateValidationMode,
            SigningCertificate = _options.SigningCertificate,
        };

        _options.SignatureValidationCertificates?.ForEach(configuration.SignatureValidationCertificates.Add);

        configuration.AllowedAudienceUris.Add(configuration.Issuer);

        var entityDescriptor = new EntityDescriptor();

        if (_idpMetadataUri.IsFile)
        {
            entityDescriptor.ReadIdPSsoDescriptorFromFile(_idpMetadataUri.ToString());
        }
        else
        {
            // await entityDescriptor.ReadIdPSsoDescriptorFromUrlAsync(_httpClientFactory, _idpMetadata, cancellationToken);
            var metadataGetResponse = await _httpClient.GetAsync(_idpMetadataUri, cancellationToken);
            metadataGetResponse.EnsureSuccessStatusCode();

            var metadataString = await metadataGetResponse.Content.ReadAsStringAsync();
            entityDescriptor.ReadIdPSsoDescriptor(metadataString);
        }

        if (entityDescriptor.IdPSsoDescriptor != null)
        {
            configuration.AllowedIssuer = entityDescriptor.EntityId;

            configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
            configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;

            foreach (var signingCertificate in entityDescriptor.IdPSsoDescriptor.SigningCertificates)
            {
                if (signingCertificate.IsValidLocalTime())
                {
                    configuration.SignatureValidationCertificates.Add(signingCertificate);
                }
            }
            if (configuration.SignatureValidationCertificates.Count == 0)
            {
                throw new Exception("The saml2 idp signing certificates has expired.");
            }
            if (entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.HasValue)
            {
                configuration.SignAuthnRequest = entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.Value;
            }
        }
        else
        {
            throw new Exception("The saml2 idp entity descriptor not loaded from metadata.");
        }

        _saml2Configuration = configuration;
        return configuration;
    }
}
