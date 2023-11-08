using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Json;

namespace Passingwind.Abp.IdentityClient.Identity;

public class OpenIdConnectEventType : OpenIdConnectEvents
{
    protected ILogger<OpenIdConnectEventType> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected IIdentityClaimMapRunner IdentityClaimMapRunner { get; }

    public OpenIdConnectEventType(
        ILogger<OpenIdConnectEventType> logger,
        IJsonSerializer jsonSerializer,
        IIdentityClientRepository identityClientRepository,
        IIdentityClaimMapRunner identityClaimMapRunner)
    {
        Logger = logger;
        JsonSerializer = jsonSerializer;
        IdentityClientRepository = identityClientRepository;
        IdentityClaimMapRunner = identityClaimMapRunner;
    }

    public override Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
    {
        var providerName = context.Scheme.Name;

        Logger.LogDebug("Authentication schame {0} received authorization code: {1}", providerName, context.ProtocolMessage.Code);

        return Task.CompletedTask;
    }

    //public override Task MessageReceived(MessageReceivedContext context)
    //{
    //    var providerName = context.Scheme.Name;

    //    Logger.LogDebug("Authentication schame {0} message received: \n{1}", providerName, "");

    //    return Task.CompletedTask;
    //}

    public override Task TokenResponseReceived(TokenResponseReceivedContext context)
    {
        var providerName = context.Scheme.Name;

        Logger.LogDebug("Authentication schame {0} received access token: \n{1}", providerName, context.TokenEndpointResponse.AccessToken);
        Logger.LogDebug("Authentication schame {0} received id token: \n{1}", providerName, context.TokenEndpointResponse.IdToken);
        Logger.LogDebug("Authentication schame {0} received refresh token: \n{1}", providerName, context.TokenEndpointResponse.RefreshToken);
        Logger.LogDebug("Authentication schame {0} received token expires: \n{1}", providerName, context.TokenEndpointResponse.ExpiresIn);

        return Task.CompletedTask;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var providerName = context.Scheme.Name;

        var principal = context.Principal;

        if (principal != null)
        {
            var cliams = principal.Claims.Select(x => new { x.Type, x.Value });
            Logger.LogDebug("Authentication schame {0} resolved claims: \n{1}", providerName, JsonSerializer.Serialize(cliams));
        }

        if (!context.Options.GetClaimsFromUserInfoEndpoint && principal?.Identity != null)
        {
            context.Principal = await HandlePrincipal(providerName, principal);
        }
    }

    public override async Task UserInformationReceived(UserInformationReceivedContext context)
    {
        var providerName = context.Scheme.Name;

        Logger.LogDebug("Authentication schame {0} received user information: \n{1}", context.User.RootElement.GetRawText());

        var principal = context.Principal;

        if (principal?.Identity != null)
        {
            context.Principal = await HandlePrincipal(providerName, principal);
        }
    }

    protected virtual async Task<ClaimsPrincipal> HandlePrincipal(string provider, ClaimsPrincipal principal)
    {
        var identityClient = await IdentityClientRepository.GetByProviderNameAsync(provider);

        var claims = principal.Claims.ToList();

        // claim map
        claims = await IdentityClaimMapRunner.RunAsync(claims, identityClient.ClaimMaps);

        // check requires
        await CheckRequirementClaims(claims, identityClient.RequiredClaimTypes);

        return new ClaimsPrincipal(new ClaimsIdentity(claims, principal.Identity?.AuthenticationType));
    }

    protected virtual Task CheckRequirementClaims(IEnumerable<Claim> claims, IEnumerable<string> requires)
    {
        if (requires?.Any() != true)
            return Task.CompletedTask;

        foreach (var type in requires)
        {
            if (!claims.Any(x => x.Type == type))
            {
                throw new UserFriendlyException($"Missing claim type '{type}'");
            }
        }

        return Task.CompletedTask;
    }
}
