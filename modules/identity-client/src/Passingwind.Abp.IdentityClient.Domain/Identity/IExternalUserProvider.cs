using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Passingwind.Abp.IdentityClient.Identity;

public interface IExternalUserProvider
{
    Task<IdentityUser?> FindUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

    Task<IdentityUser> CreateUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

    Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser, ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default);
}
