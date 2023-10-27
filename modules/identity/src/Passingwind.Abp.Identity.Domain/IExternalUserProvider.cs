using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

/// <summary>
///  External user provider
/// </summary>
public interface IExternalUserProvider
{
    Task<IdentityUser?> FindUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

    Task<IdentityUser> CreateUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, string? loginDisplayName = null, bool? generateUserName = false, CancellationToken cancellationToken = default);

    Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser, ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
