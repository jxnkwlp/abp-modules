using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ITfoxtec.Identity.Saml2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Passingwind.Authentication.Saml2;

public class Saml2Events : RemoteAuthenticationEvents
{
    /// <summary>
    /// Invoked when a protocol message is first received.
    /// </summary>
    public Func<MessageReceivedContext, Task> OnMessageReceived { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
    /// </summary>
    public Func<AuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked to manipulate redirects to the identity provider for SignIn, SignOut, or Challenge.
    /// </summary>
    public Func<RedirectContext, Task> OnRedirectToIdentityProvider { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked when a wsignoutcleanup request is received at the RemoteSignOutPath endpoint.
    /// </summary>
    public Func<RemoteSignOutContext, Task> OnRemoteSignOut { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked with the security token that has been extracted from the protocol message.
    /// </summary>
    public Func<SecurityTokenReceivedContext, Task> OnSecurityTokenReceived { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
    /// </summary>
    public Func<SecurityTokenValidatedContext, Task> OnSecurityTokenValidated { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task AuthenticationFailed(AuthenticationFailedContext context) => OnAuthenticationFailed(context);

    /// <summary>
    /// Invoked to manipulate redirects to the identity provider for SignIn, SignOut, or Challenge.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task RedirectToIdentityProvider(RedirectContext context) => OnRedirectToIdentityProvider(context);

    /// <summary>
    /// Invoked when a protocol message is first received.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task MessageReceived(MessageReceivedContext context) => OnMessageReceived(context);

    /// <summary>
    /// Invoked when a wsignoutcleanup request is received at the RemoteSignOutPath endpoint.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task RemoteSignOut(RemoteSignOutContext context) => OnRemoteSignOut(context);

    /// <summary>
    /// Invoked with the security token that has been extracted from the protocol message.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task SecurityTokenReceived(SecurityTokenReceivedContext context) => OnSecurityTokenReceived(context);

    /// <summary>
    /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
    /// </summary>
    /// <param name="context"></param>
    public virtual Task SecurityTokenValidated(SecurityTokenValidatedContext context) => OnSecurityTokenValidated(context);
}

public class RedirectContext : PropertiesContext<Saml2Options>
{
    public RedirectContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options, AuthenticationProperties? properties) : base(context, scheme, options, properties)
    {
    }

    public Saml2AuthnRequest Saml2AuthnRequest { get; set; } = default!;

    public Saml2RedirectBinding RedirectBinding { get; set; } = default!;

    /// <summary>
    /// If true, will skip any default logic for this redirect.
    /// </summary>
    public bool Handled { get; private set; }

    /// <summary>
    /// Skips any default logic for this redirect.
    /// </summary>
    public void HandleResponse() => Handled = true;
}

public class RemoteSignOutContext : RemoteAuthenticationContext<Saml2Options>
{
    public RemoteSignOutContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options, AuthenticationProperties? properties) : base(context, scheme, options, properties)
    {
    }

    public Saml2AuthnResponse Saml2AuthnResponse { get; set; } = default!;
}

public class MessageReceivedContext : RemoteAuthenticationContext<Saml2Options>
{
    public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options, AuthenticationProperties? properties) : base(context, scheme, options, properties)
    {
    }

    public Saml2AuthnResponse Saml2AuthnResponse { get; set; } = default!;
}

public class SecurityTokenReceivedContext : RemoteAuthenticationContext<Saml2Options>
{
    public SecurityTokenReceivedContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options, AuthenticationProperties? properties) : base(context, scheme, options, properties)
    {
    }

    public Saml2AuthnResponse Saml2AuthnResponse { get; set; } = default!;
}

public class SecurityTokenValidatedContext : RemoteAuthenticationContext<Saml2Options>
{
    public SecurityTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options, ClaimsPrincipal principal, AuthenticationProperties? properties) : base(context, scheme, options, properties)
    {
        Principal = principal;
    }

    public Saml2AuthnResponse Saml2AuthnResponse { get; set; } = default!;
}

public class AuthenticationFailedContext : RemoteAuthenticationContext<Saml2Options>
{
    public AuthenticationFailedContext(HttpContext context, AuthenticationScheme scheme, Saml2Options options) : base(context, scheme, options, null)
    {
    }

    public Saml2AuthnResponse Saml2AuthnResponse { get; set; } = default!;

    public Exception Exception { get; set; } = default!;
}
