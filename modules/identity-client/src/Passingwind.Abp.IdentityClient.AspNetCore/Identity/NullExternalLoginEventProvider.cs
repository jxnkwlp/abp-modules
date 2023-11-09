using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class NullExternalLoginEventProvider : IExternalLoginEventProvider, ISingletonDependency
{
    public virtual Task LoginInfoReceivedAsync(ExternalLoginInfoReceivedContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task MessageReceivedAsync(ExternalLoginMessageReceivedContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task RedirectToIdentityProviderAsync(ExternalLoginRedirectToIdentityProviderContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task SignInAsync(ExternalLoginSignInContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task UserSignInAsync(ExternalLoginUserSignInContext context)
    {
        return Task.CompletedTask;
    }
}
