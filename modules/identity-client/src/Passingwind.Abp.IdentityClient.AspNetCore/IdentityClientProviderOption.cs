using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Passingwind.AspNetCore.Authentication.Saml2;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientProviderOption
{
    public Action<OpenIdConnectOptions>? ConfigureOpenIdConnectOptionDefault { get; set; }
    public Action<string, OpenIdConnectOptions>? ConfigureOpenIdConnectOption { get; set; }

    public Action<Saml2Options>? ConfigureSaml2OptionsDefault { get; set; }
    public Action<string, Saml2Options>? ConfigureSaml2OptionsOption { get; set; }

    public string? LoggedOutUrl { get; set; }
}
