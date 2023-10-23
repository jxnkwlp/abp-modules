using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Passingwind.Abp.IdentityClient.Cryptography;
using Passingwind.Abp.IdentityClient.Identity;
using Passingwind.AspNetCore.Authentication.Saml2;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Saml2;

public class Saml2OptionBuilder : ISaml2OptionBuilder, ITransientDependency
{
    protected ICertificateLoader CertificateLoader { get; }
    protected Saml2PostConfigureOptions Saml2PostConfigureOptions { get; }

    public Saml2OptionBuilder(ICertificateLoader certificateLoader, Saml2PostConfigureOptions saml2PostConfigureOptions)
    {
        CertificateLoader = certificateLoader;
        Saml2PostConfigureOptions = saml2PostConfigureOptions;
    }

    public async Task<Saml2Options> GetAsync(string provider, IdentityClientSaml2Configuration configuration, CancellationToken cancellationToken = default)
    {
        var options = new Saml2Options()
        {
            CallbackPath = new PathString("/auth/signin-saml2"),
            RemoteSignOutPath = new PathString("/auth/signout-saml2"),
            EventsType = typeof(Saml2EventType),
            Issuer = configuration.Issuer ?? provider,
            ForceAuthn = configuration.ForceAuthn ?? true,
        };

        if (configuration.TrustCertificate.HasValue)
            options.CertificateValidationMode = configuration.TrustCertificate.Value ? System.ServiceModel.Security.X509CertificateValidationMode.None : System.ServiceModel.Security.X509CertificateValidationMode.PeerOrChainTrust;

        if (!string.IsNullOrWhiteSpace(configuration.CallbackPath))
        {
            options.CallbackPath = new PathString(configuration.CallbackPath);
        }
        if (!string.IsNullOrWhiteSpace(configuration.RemoteSignOutPath))
        {
            options.RemoteSignOutPath = new PathString(configuration.RemoteSignOutPath);
        }

        if (!string.IsNullOrWhiteSpace(configuration.IdpMetadataUrl))
        {
            options.IdpMetadataUri = new Uri(configuration.IdpMetadataUrl);
        }
        else if (!string.IsNullOrWhiteSpace(configuration.IdpMetadataContent))
        {
            // save to tmp
            var tmpFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tmpFile, configuration.IdpMetadataContent, cancellationToken);
            options.IdpMetadataUri = new Uri(tmpFile);
        }

        if (!string.IsNullOrWhiteSpace(configuration.SigningCertificatePem))
        {
            options.SigningCertificate = CertificateLoader.Create(configuration.SigningCertificatePem, configuration.SigningCertificateKeyPem);
        }

        Saml2PostConfigureOptions.PostConfigure(provider, options);

        return options;
    }
}
