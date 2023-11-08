using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient.Cryptography;
using Passingwind.Abp.IdentityClient.Identity;
using Passingwind.AspNetCore.Authentication.Saml2;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Saml2;

public class Saml2OptionBuilder : ISaml2OptionBuilder, ITransientDependency
{
    protected ICertificateLoader CertificateLoader { get; }
    protected IdentityClientProviderOption IdentityClientProviderOption { get; }

    public Saml2OptionBuilder(ICertificateLoader certificateLoader, IOptions<IdentityClientProviderOption> identityClientProviderOption)
    {
        CertificateLoader = certificateLoader;
        IdentityClientProviderOption = identityClientProviderOption.Value;
    }

    public async Task<Saml2Options> GetAsync(string provider, IdentityClientSaml2Configuration configuration, CancellationToken cancellationToken = default)
    {
        var options = new Saml2Options()
        {
            CallbackPath = new PathString(string.Format(Saml2Consts.SignInPathFormat, provider)),
            RemoteSignOutPath = new PathString(string.Format(Saml2Consts.SignOutPathFormat, provider)),
            EventsType = typeof(Saml2EventType),
            Issuer = !string.IsNullOrWhiteSpace(configuration.Issuer) ? configuration.Issuer : provider,
            ForceAuthn = configuration.ForceAuthn ?? true,
        };

        IdentityClientProviderOption.ConfigureSaml2OptionsDefault?.Invoke(options);

        options.CertificateValidationMode = configuration.TrustCertificate == true ? System.ServiceModel.Security.X509CertificateValidationMode.None : System.ServiceModel.Security.X509CertificateValidationMode.PeerOrChainTrust;

        if (!string.IsNullOrWhiteSpace(configuration.CallbackPath))
        {
            options.CallbackPath = new PathString(configuration.CallbackPath);
        }
        if (!string.IsNullOrWhiteSpace(configuration.SignOutPath))
        {
            options.RemoteSignOutPath = new PathString(configuration.SignOutPath);
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

        IdentityClientProviderOption.ConfigureSaml2OptionsOption?.Invoke(provider, options);

        return options;
    }
}
