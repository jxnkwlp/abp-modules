using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.IdentityClient;

public interface IIdentityClientProviderNameProvider
{
    Task<string> CreateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default);
}
