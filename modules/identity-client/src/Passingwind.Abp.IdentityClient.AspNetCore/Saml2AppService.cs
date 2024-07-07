using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClient.Cryptography;
using Passingwind.Abp.IdentityClient.Saml2;
using Volo.Abp.Application.Services;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClient;

public class Saml2AppService : ApplicationService, ISaml2AppService
{
    protected HttpContext? HttpContext { get; }
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected ICertificateLoader CertificateLoader { get; }
    protected IdentityClientProviderOption IdentityClientProviderOption { get; }
    protected ISaml2OptionBuilder Saml2OptionBuilder { get; }
    protected ISaml2ConfigurationBuilder Saml2ConfigurationBuilder { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }

    public Saml2AppService(
        IIdentityClientRepository identityClientRepository,
        ICertificateLoader certificateLoader,
        IOptions<IdentityClientProviderOption> identityClientProviderOption,
        ISaml2OptionBuilder saml2OptionBuilder,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor httpContextAccessor,
        ISaml2ConfigurationBuilder saml2ConfigurationBuilder)
    {
        IdentityClientRepository = identityClientRepository;
        CertificateLoader = certificateLoader;
        IdentityClientProviderOption = identityClientProviderOption.Value;
        Saml2OptionBuilder = saml2OptionBuilder;
        SignInManager = signInManager;
        HttpContext = httpContextAccessor.HttpContext;
        Saml2ConfigurationBuilder = saml2ConfigurationBuilder;
    }

    [AllowAnonymous]
    public virtual async Task<string> GetMetadataDescriptorAsync(Uri baseUri, string name)
    {
        var identityClient = await IdentityClientRepository.GetByNameAsync(name);
        var configuration = IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations);

        var saml2Options = await Saml2OptionBuilder.GetAsync(identityClient.ProviderName, configuration);

        var saml2Configuration = await Saml2ConfigurationBuilder.GetAsync(saml2Options, configuration);

        X509Certificate2? signingCert = null;
        if (!string.IsNullOrWhiteSpace(configuration.SigningCertificatePem))
            signingCert = CertificateLoader.Create(configuration.SigningCertificatePem, configuration.SigningCertificateKeyPem);

        var signOutPath = (string.IsNullOrWhiteSpace(configuration.SignOutPath) ? string.Format(Saml2Consts.SignOutPathFormat, identityClient.ProviderName) : configuration.SignOutPath).RemovePreFix("/");
        var spSsoSingleLogoutService = new SingleLogoutService
        {
            Binding = configuration.UseGetAsSingleLogoutService == true ? ProtocolBindings.HttpRedirect : ProtocolBindings.HttpPost,
            Location = new Uri(baseUri, signOutPath),
        };

        if (!string.IsNullOrWhiteSpace(IdentityClientProviderOption.LoggedOutUrl))
        {
            spSsoSingleLogoutService.ResponseLocation = new Uri(baseUri, IdentityClientProviderOption.LoggedOutUrl);
        }

        var signInPath = (string.IsNullOrWhiteSpace(configuration.CallbackPath) ? string.Format(Saml2Consts.SignInPathFormat, identityClient.ProviderName) : configuration.CallbackPath).RemovePreFix("/");
        var spSsoAssertionConsumerService = new AssertionConsumerService
        {
            Binding = configuration.UseGetAsAssertionConsumerService == true ? ProtocolBindings.HttpRedirect : ProtocolBindings.HttpPost,
            Location = new Uri(baseUri, signInPath),
            IsDefault = true,
        };

        var spSsoDescriptor = new SPSsoDescriptor
        {
            AuthnRequestsSigned = saml2Configuration.SignAuthnRequest,
            WantAssertionsSigned = configuration.RequireAssertionsSigned,
            SingleLogoutServices = new SingleLogoutService[] { spSsoSingleLogoutService },
            AssertionConsumerServices = new AssertionConsumerService[] { spSsoAssertionConsumerService },
            NameIDFormats = new Uri[]
            {
                NameIdentifierFormats.Persistent
            },
            AttributeConsumingServices = new AttributeConsumingService[]
            {
                new AttributeConsumingService
                {
                     ServiceNames = new []{ new LocalizedNameType(identityClient.Name, "en") },
                    RequestedAttributes = CreateRequestedAttributes(),
                }
            },
        };

        if (!string.IsNullOrWhiteSpace(configuration.NameIDFormats))
        {
            var uris = new List<Uri>();
            var formats = configuration.NameIDFormats.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (formats.Contains(nameof(NameIdentifierFormats.Transient)))
                uris.Add(NameIdentifierFormats.Transient);
            if (formats.Contains(nameof(NameIdentifierFormats.Persistent)))
                uris.Add(NameIdentifierFormats.Persistent);
            if (formats.Contains(nameof(NameIdentifierFormats.Email)))
                uris.Add(NameIdentifierFormats.Email);
            if (formats.Contains(nameof(NameIdentifierFormats.X509SubjectName)))
                uris.Add(NameIdentifierFormats.X509SubjectName);
            if (formats.Contains(nameof(NameIdentifierFormats.Unspecified)))
                uris.Add(NameIdentifierFormats.Unspecified);

            spSsoDescriptor.NameIDFormats = uris;
        }

        if (signingCert != null)
        {
            spSsoDescriptor.SigningCertificates = new List<X509Certificate2>() { signingCert };
        }

        var entityDescriptor = new EntityDescriptor(saml2Configuration)
        {
            ValidUntil = 365,
            SPSsoDescriptor = spSsoDescriptor,
        };

        var metadata = new Saml2Metadata(entityDescriptor).CreateMetadata();

        return metadata.ToXml();
    }

    [Authorize]
    public virtual async Task<string> LogoutAsync(string name)
    {
        if (HttpContext == null)
            throw new ArgumentException(nameof(HttpContext));

        var identityClient = await IdentityClientRepository.GetByNameAsync(name);
        var configuration = IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations);

        var saml2Options = await Saml2OptionBuilder.GetAsync(identityClient.ProviderName, configuration);

        var saml2Configuration = await Saml2ConfigurationBuilder.GetAsync(saml2Options, configuration);

        var binding = new Saml2PostBinding();
        var logoutRequest = new Saml2LogoutRequest(saml2Configuration, HttpContext.User);

        // logout current session
        await SignInManager.SignOutAsync();

        // remote logout
        binding = binding.Bind(logoutRequest);

        return binding.PostContent;
    }

    private IEnumerable<RequestedAttribute> CreateRequestedAttributes()
    {
        yield return new RequestedAttribute("urn:oid:2.5.4.4");
        yield return new RequestedAttribute("urn:oid:2.5.4.3", false);
    }
}
