using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Passingwind.Authentication.Saml2.Configuration;

namespace Passingwind.Authentication.Saml2;

public class Saml2Options : RemoteAuthenticationOptions
{
    public string Issuer { get; set; } = null!;

    public NameIdPolicy? NameIdPolicy { get; set; }

    public string SignOutScheme { get; set; } = null!;

    public PathString RemoteSignOutPath { get; set; }

    public bool ForceAuthn { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool SaveTokens { get; set; }

    public new Saml2Events Events
    {
        get => (Saml2Events)base.Events;
        set => base.Events = value;
    }

    public Uri? IdpMetadataUri { get; set; }

    public X509Certificate2? SigningCertificate { get; set; }

    public List<X509Certificate2>? SignatureValidationCertificates { get; set; }

    public X509CertificateValidationMode CertificateValidationMode { get; set; }

    public Saml2Configuration Configuration { get; set; } = null!;

    public IConfigurationManager ConfigurationManager { get; set; } = default!;

    public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; } = default!;

    public Saml2Options()
    {
        Events = new Saml2Events();
        CallbackPath = new PathString("/signin-saml2");
        RemoteSignOutPath = new PathString("/signout-saml2");
        CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

        //Saml2RequestConfigure = (config) =>
        //{
        //    config.ForceAuthn = ForceAuthn;
        //    //config.NameIdPolicy = new NameIdPolicy
        //    //{
        //    //    AllowCreate = true,
        //    //    Format = "urn:oasis:names:tc:SAML:2.0:nameid-form.at:persistent"
        //    //};
        //    config.RequestedAuthnContext = new RequestedAuthnContext
        //    {
        //        Comparison = AuthnContextComparisonTypes.Exact,
        //        AuthnContextClassRef = new string[] { AuthnContextClassTypes.PasswordProtectedTransport.OriginalString },
        //    };
        //};
    }

    public override void Validate()
    {
        base.Validate();

        if (ConfigurationManager == null)
        {
            throw new InvalidOperationException($"Provide {nameof(IdpMetadataUri)}, {nameof(Configuration)}, or {nameof(ConfigurationManager)} to {nameof(Saml2Options)}");
        }
    }
}
