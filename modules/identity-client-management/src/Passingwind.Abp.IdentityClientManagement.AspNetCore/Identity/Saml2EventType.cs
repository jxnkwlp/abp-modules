using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Passingwind.Authentication.Saml2;
using Volo.Abp;
using Volo.Abp.Json;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class Saml2EventType : Saml2Events
{
    protected ILogger<OpenIdConnectEventType> Logger { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IIdentityClientRepository IdentityClientRepository { get; }
    protected IIdentityClaimMapRunner IdentityClaimMapRunner { get; }

    public Saml2EventType(
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

    public override Task RedirectToIdentityProvider(RedirectContext context)
    {
        var providerName = context.Scheme.Name;

        var xml = context.RedirectBinding.XmlDocument.OuterXml;

        Logger.LogInformation("Authentication schame {0} request identity provider payload: {1}", providerName, xml);
        Logger.LogInformation("Authentication schame {0} redirect to identity provider url: {1}", providerName, context.RedirectBinding.RedirectLocation.OriginalString);

        return Task.CompletedTask;
    }

    public override Task MessageReceived(MessageReceivedContext context)
    {
        var providerName = context.Scheme.Name;

        var xml = context.Saml2AuthnResponse.ToXml().OuterXml;

        Logger.LogInformation("Authentication schame {0} received response payload: {1}", providerName, xml);

        return Task.CompletedTask;
    }

    public override async Task SecurityTokenValidated(SecurityTokenValidatedContext context)
    {
        var providerName = context.Scheme.Name;

        var principal = context.Principal;

        if (principal != null)
        {
            var cliams = principal.Claims.Select(x => new { x.Type, x.Value });
            Logger.LogInformation("Authentication schame {0} resolved claims: {1}", providerName, JsonSerializer.Serialize(cliams));

            // 
            var claimsIdentity = await HandlePrincipal(providerName, principal);

            context.Principal = new ClaimsPrincipal(claimsIdentity);
        }
    }

    protected virtual async Task<ClaimsIdentity> HandlePrincipal(string provider, ClaimsPrincipal principal)
    {
        var identityClient = await IdentityClientRepository.GetByNameAsync(provider);

        var claims = principal.Claims.ToList();

        // claim map
        claims = await IdentityClaimMapRunner.RunAsync(claims, identityClient.ClaimMaps);

        // check requires
        await CheckRequirementClaims(claims, identityClient.RequiredClaimTypes);

        return new ClaimsIdentity(claims, principal.Identity?.AuthenticationType);
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
