using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class NullIdentityClientRegisterProvider : IIdentityClientRegisterProvider, ITransientDependency
{
    public virtual Task RegisterAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task RegisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task UnregisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task ValidateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
