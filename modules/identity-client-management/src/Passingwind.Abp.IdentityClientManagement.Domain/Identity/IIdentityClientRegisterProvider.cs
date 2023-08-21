using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public interface IIdentityClientRegisterProvider
{
    Task RegisterAllAsync(CancellationToken cancellationToken = default);
    Task RegisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default);
    Task UnregisterAsync(IdentityClient identityClient, CancellationToken cancellationToken = default);
    Task ValidateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default);
}
