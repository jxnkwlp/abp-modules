using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Passingwind.Abp.IdentityClientManagement.Cryptography;
using Passingwind.Abp.IdentityClientManagement.Identity;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Passingwind.Abp.IdentityClientManagement.Saml2;
using Volo.Abp.AspNetCore.Mvc;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Passingwind.Abp.IdentityClientManagement.Controllers;

[Area("IdentityClientManagement")]
[Route("auth/saml2")]
public class Saml2Controller : AbpController
{
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected ICertificateLoader CertificateLoader { get; }
    protected IdentityClientProviderOption IdentityClientProviderOption { get; }
    protected ISaml2OptionBuilder Saml2OptionBuilder { get; }
    protected SignInManager<IdentityUser> SignInManager { get; }

    public Saml2Controller(
        IIdentityClientRepository identityClientRepository,
        ICertificateLoader certificateLoader,
        IOptions<IdentityClientProviderOption> identityClientProviderOption,
        ISaml2OptionBuilder saml2OptionBuilder,
        SignInManager<IdentityUser> signInManager)
    {
        IdentityClientRepository = identityClientRepository;
        CertificateLoader = certificateLoader;
        IdentityClientProviderOption = identityClientProviderOption.Value;
        Saml2OptionBuilder = saml2OptionBuilder;
        SignInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpGet("{name}/metadata")]
    [HttpGet("{name}/endpoint/descriptor")]
    public async Task<IActionResult> Saml2MetadataAsync(string name)
    {
        var defaultSite = new Uri($"{Request.Scheme}://{Request.Host.ToUriComponent()}/");

        var identityClient = await IdentityClientRepository.GetByNameAsync(name);
        var configuration = IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations);

        var saml2Options = await Saml2OptionBuilder.GetAsync(identityClient.ProviderName, configuration);

        saml2Options.Configuration = await saml2Options.ConfigurationManager.GetConfigurationAsync();

        X509Certificate2? signingCert = null;
        if (!string.IsNullOrWhiteSpace(configuration.SigningCertificatePem))
            signingCert = CertificateLoader.Create(configuration.SigningCertificatePem, configuration.SigningCertificateKeyPem);

        var spSsoSingleLogoutService = new SingleLogoutService
        {
            Binding = configuration.UseGetAsSingleLogoutService == true ? ProtocolBindings.HttpRedirect : ProtocolBindings.HttpPost,
            Location = new Uri(defaultSite, string.IsNullOrWhiteSpace(configuration.RemoteSignOutPath) ? "auth/signout-saml2" : configuration.RemoteSignOutPath),
        };

        if (!string.IsNullOrWhiteSpace(IdentityClientProviderOption.LoggedOutUrl))
        {
            spSsoSingleLogoutService.ResponseLocation = new Uri(defaultSite, IdentityClientProviderOption.LoggedOutUrl);
        }

        var spSsoAssertionConsumerService = new AssertionConsumerService
        {
            Binding = configuration.UseGetAsAssertionConsumerService == true ? ProtocolBindings.HttpRedirect : ProtocolBindings.HttpPost,
            Location = new Uri(defaultSite, string.IsNullOrWhiteSpace(configuration.CallbackPath) ? "auth/signin-saml2" : configuration.CallbackPath),
            IsDefault = true,
        };

        var spSsoDescriptor = new SPSsoDescriptor
        {
            AuthnRequestsSigned = configuration.AuthnRequestsSigned,
            WantAssertionsSigned = configuration.RequireAssertionsSigned,
            SingleLogoutServices = new SingleLogoutService[] { spSsoSingleLogoutService },
            AssertionConsumerServices = new AssertionConsumerService[] { spSsoAssertionConsumerService },
            NameIDFormats = new Uri[] {
                NameIdentifierFormats.Persistent
            },
            // EncryptionCertificates = 
            //AttributeConsumingServices = new AttributeConsumingService[]
            //{
            //    new AttributeConsumingService {
            //        ServiceName = new ServiceName("Some SP", "en"),
            //        RequestedAttributes = CreateRequestedAttributes()
            //    }
            //},
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

        //entityDescriptor.ContactPersons = new[] {
        //    new ContactPerson(ContactTypes.Administrative)
        //    {
        //        Company = "Some Company",
        //        GivenName = "Some Given Name",
        //        SurName = "Some Sur Name",
        //        EmailAddress = "some@some-domain.com",
        //        TelephoneNumber = "11111111",
        //    },
        //    new ContactPerson(ContactTypes.Technical)
        //    {
        //        Company = "Some Company",
        //        GivenName = "Some tech Given Name",
        //        SurName = "Some tech Sur Name",
        //        EmailAddress = "sometech@some-domain.com",
        //        TelephoneNumber = "22222222",
        //    }
        //};

        var entityDescriptor = new EntityDescriptor(saml2Options.Configuration)
        {
            ValidUntil = 365,
            SPSsoDescriptor = spSsoDescriptor,
        };

        var metadata = new Saml2Metadata(entityDescriptor).CreateMetadata();

        return Content(metadata.ToXml(), "text/xml", Encoding.UTF8);
    }

    [Authorize]
    [HttpPost("{name}/logout")]
    public async Task<IActionResult> Logout(string name)
    {
        var identityClient = await IdentityClientRepository.GetByNameAsync(name);
        var configuration = IdentityClientConfigurationHelper.ToSaml2Configuration(identityClient.Configurations);

        var saml2Options = await Saml2OptionBuilder.GetAsync(identityClient.Name, configuration);

        saml2Options.Configuration = await saml2Options.ConfigurationManager.GetConfigurationAsync();

        var binding = new Saml2PostBinding();
        var logoutRequest = new Saml2LogoutRequest(saml2Options.Configuration, User);

        // logout current session
        await SignInManager.SignOutAsync();

        // remote logout
        binding = binding.Bind(logoutRequest);

        return new ContentResult
        {
            ContentType = "text/html",
            Content = binding.PostContent
        };
    }
}
