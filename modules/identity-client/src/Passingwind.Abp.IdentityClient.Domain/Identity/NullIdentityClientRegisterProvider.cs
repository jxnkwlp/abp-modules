using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.IdentityClient.Identity;

public class NullIdentityClientRegisterProvider : IIdentityClientRegisterProvider, ITransientDependency
{
    public Task RegisterAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RegisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task UnregisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task ValidateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
