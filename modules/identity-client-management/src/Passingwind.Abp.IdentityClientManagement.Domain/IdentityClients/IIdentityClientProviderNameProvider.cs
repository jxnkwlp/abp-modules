using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public interface IIdentityClientProviderNameProvider
{
    Task<string> CreateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default);
}
