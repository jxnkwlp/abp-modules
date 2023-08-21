using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Passingwind.Abp.IdentityClientManagement.Identity;

public class NullExternalUserProvider : IExternalUserProvider, ITransientDependency
{
    public Task<IdentityUser> CreateUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<IdentityUser?> FindUserAsync(ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }

    public Task<IdentityUser> UpdateUserAsync(IdentityUser identityUser, ClaimsPrincipal principal, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException();
    }
}
