using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class NullExternalLoginEventProvider : IExternalLoginEventProvider, ISingletonDependency
{
    public Task LoginInfoReceivedAsync(ExternalLoginInfoReceivedContext context)
    {
        return Task.CompletedTask;
    }

    public Task MessageReceivedAsync(ExternalLoginMessageReceivedContext context)
    {
        return Task.CompletedTask;
    }

    public Task RedirectToIdentityProviderAsync(ExternalLoginRedirectToIdentityProviderContext context)
    {
        return Task.CompletedTask;
    }

    public Task SignInAsync(ExternalLoginSignInContext context)
    {
        return Task.CompletedTask;
    }

    public Task UserSignInAsync(ExternalLoginUserSignInContext context)
    {
        return Task.CompletedTask;
    }
}
