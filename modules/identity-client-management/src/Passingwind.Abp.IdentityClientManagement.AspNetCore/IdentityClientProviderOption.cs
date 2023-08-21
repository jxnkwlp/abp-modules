using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Passingwind.Abp.IdentityClientManagement;

public class IdentityClientProviderOption
{
    public Action<OpenIdConnectOptions>? ConfigureOpenIdConnectOptionDefault { get; set; }
    public Action<string, OpenIdConnectOptions>? ConfigureOpenIdConnectOption { get; set; }

    public string? LoggedOutUrl { get; set; }
}
