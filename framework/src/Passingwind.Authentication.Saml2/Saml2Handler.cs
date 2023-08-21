using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Passingwind.Authentication.Saml2;

public class Saml2Handler : RemoteAuthenticationHandler<Saml2Options>, IAuthenticationSignOutHandler
{
    private Saml2Configuration? _configuration;
    private const string RelayStateName = "State";
    private const string CorrelationProperty = ".xsrf";

    protected new Saml2Events Events
    {
        get { return (Saml2Events)base.Events; }
        set { base.Events = value; }
    }

    public Saml2Handler(IOptionsMonitor<Saml2Options> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        properties ??= new AuthenticationProperties();

        if (_configuration == null)
        {
            _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
        }

        // Save the original challenge URI so we can redirect back to it when we're done.
        if (string.IsNullOrEmpty(properties.RedirectUri))
        {
            properties.RedirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
        }

        var saml2AuthnRequest = new Saml2AuthnRequest(_configuration);
        saml2AuthnRequest.ForceAuthn = Options.ForceAuthn;
        saml2AuthnRequest.NameIdPolicy = Options.NameIdPolicy;
        saml2AuthnRequest.RequestedAuthnContext = new RequestedAuthnContext
        {
            Comparison = AuthnContextComparisonTypes.Exact,
            AuthnContextClassRef = new string[] { AuthnContextClassTypes.PasswordProtectedTransport.OriginalString },
        };

        var relayStateQuery = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(properties.RedirectUri))
            relayStateQuery[Options.ReturnUrlParameter] = properties.RedirectUri;

        relayStateQuery[RelayStateName] = Options.StateDataFormat.Protect(properties);

        GenerateCorrelationId(properties);

        var binding = new Saml2RedirectBinding();
        binding.SetRelayStateQuery(relayStateQuery);

        binding = binding.Bind(saml2AuthnRequest);

        var redirectContext = new RedirectContext(Context, Scheme, Options, properties)
        {
            Saml2AuthnRequest = saml2AuthnRequest,
            RedirectBinding = binding,
        };

        await Events.RedirectToIdentityProvider(redirectContext);

        if (redirectContext.Handled)
        {
            return;
        }

        binding = redirectContext.RedirectBinding;

        Response.Redirect(binding.RedirectLocation.OriginalString);
    }

    public Task SignOutAsync(AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        if (_configuration == null)
        {
            _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
        }

        var saml2AuthnResponse = new Saml2AuthnResponse(_configuration);

        AuthenticationProperties? properties = null;

        var relayStateQuery = new Dictionary<string, string>();

        try
        {
            if (Request.Method == HttpMethods.Get)
            {
                var binding = new Saml2RedirectBinding();

                var saml2Request = Request.ToGenericHttpRequest();

                binding.ReadSamlResponse(saml2Request, saml2AuthnResponse);

                relayStateQuery = binding.GetRelayStateQuery();
            }
            else if (Request.Method == HttpMethods.Post)
            {
                var binding = new Saml2PostBinding();

                var saml2Request = Request.ToGenericHttpRequest();

                binding.ReadSamlResponse(saml2Request, saml2AuthnResponse);

                relayStateQuery = binding.GetRelayStateQuery();
            }
            else
            {
                throw new AuthenticationException($"Saml2 response method '{Request.Method}' not support");
            }

            if (!relayStateQuery.ContainsKey(RelayStateName))
            {
                throw new AuthenticationException("Saml2 response missing relay state ");
            }

            var state = relayStateQuery[RelayStateName];

            properties = Options.StateDataFormat.Unprotect(state);

            var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options, properties)
            {
                Saml2AuthnResponse = saml2AuthnResponse
            };
            await Events.MessageReceived(messageReceivedContext);
            if (messageReceivedContext.Result != null)
            {
                return messageReceivedContext.Result;
            }

            saml2AuthnResponse = messageReceivedContext.Saml2AuthnResponse;
            properties = messageReceivedContext.Properties!; // Provides a new instance if not set.

            // If state did flow from the challenge then validate it. See AllowUnsolicitedLogins above.
            if (properties.Items.TryGetValue(CorrelationProperty, out string? correlationId)
                && !ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                return HandleRequestResult.Fail($"Saml2 response status: {saml2AuthnResponse.Status}", properties);
            }

            ClaimsPrincipal? principal = new ClaimsPrincipal(saml2AuthnResponse.ClaimsIdentity);

            var securityTokenReceivedContext = new SecurityTokenReceivedContext(Context, Scheme, Options, properties)
            {
                Saml2AuthnResponse = saml2AuthnResponse
            };
            await Events.SecurityTokenReceived(securityTokenReceivedContext);
            if (securityTokenReceivedContext.Result != null)
            {
                return securityTokenReceivedContext.Result;
            }

            var securityTokenValidatedContext = new SecurityTokenValidatedContext(Context, Scheme, Options, principal, properties)
            {
                Saml2AuthnResponse = saml2AuthnResponse,
            };

            await Events.SecurityTokenValidated(securityTokenValidatedContext);
            if (securityTokenValidatedContext.Result != null)
            {
                return securityTokenValidatedContext.Result;
            }

            // Flow possible changes
            principal = securityTokenValidatedContext.Principal!;
            properties = securityTokenValidatedContext.Properties;

            return HandleRequestResult.Success(new AuthenticationTicket(principal, properties, Scheme.Name));
        }
        catch (System.Exception ex)
        {
            Logger.LogError(ex, "Exception occurred while processing message");

            var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
            {
                Saml2AuthnResponse = saml2AuthnResponse,
                Exception = ex
            };

            await Events.AuthenticationFailed(authenticationFailedContext);

            return authenticationFailedContext.Result ?? HandleRequestResult.Fail(ex, properties);
        }
    }
}
