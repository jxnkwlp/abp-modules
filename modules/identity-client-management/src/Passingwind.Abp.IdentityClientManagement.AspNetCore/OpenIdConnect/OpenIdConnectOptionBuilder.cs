using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Passingwind.Abp.IdentityClientManagement.Identity;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClientManagement.OpenIdConnect;

public class OpenIdConnectOptionBuilder : IOpenIdConnectOptionBuilder, ITransientDependency
{
    protected IdentityClientProviderOption IdentityClientProviderOption { get; }

    public OpenIdConnectOptionBuilder(IOptions<IdentityClientProviderOption> identityClientProviderOption)
    {
        IdentityClientProviderOption = identityClientProviderOption.Value;
    }

    public Task<OpenIdConnectOptions> GetAsync(string provider, IdentityClientOpenIdConnectConfiguration configuration, CancellationToken cancellationToken = default)
    {
        OpenIdConnectOptions options = new OpenIdConnectOptions
        {
            RequireHttpsMetadata = false,
            GetClaimsFromUserInfoEndpoint = true,
            SaveTokens = true,
            ResponseType = OpenIdConnectResponseType.Code,
            MapInboundClaims = true,
            CallbackPath = "/auth/signin-oidc",
            EventsType = typeof(OpenIdConnectEventType)
        };

        options.ClaimActions.MapAbpClaimTypes();

        IdentityClientProviderOption.ConfigureOpenIdConnectOptionDefault?.Invoke(options);

        options.Authority = configuration.Authority;
        options.ClientId = configuration.ClientId;
        options.ClientSecret = configuration.ClientSecret;
        options.RequireHttpsMetadata = configuration.RequireHttpsMetadata ?? options.RequireHttpsMetadata;
        options.MetadataAddress = configuration.MetadataAddress;
        options.ResponseMode = configuration.ResponseMode ?? options.ResponseMode;
        options.ResponseType = configuration.ResponseType ?? options.ResponseType;
        options.UsePkce = configuration.UsePkce ?? options.UsePkce;
        options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint ?? options.GetClaimsFromUserInfoEndpoint;

        string[]? scopes = configuration.Scope?.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (scopes?.Any() == true)
        {
            options.Scope.Clear();
            scopes!.ToList().ForEach(options.Scope.Add);
        }

        IdentityClientProviderOption.ConfigureOpenIdConnectOption?.Invoke(provider, options);

        return Task.FromResult(options);
    }
}
