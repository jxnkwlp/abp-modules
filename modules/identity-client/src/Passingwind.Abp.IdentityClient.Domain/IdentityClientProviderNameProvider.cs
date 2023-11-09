using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientProviderNameProvider : IIdentityClientProviderNameProvider, ITransientDependency
{
    protected ITenantStore TenantStore { get; }

    public IdentityClientProviderNameProvider(ITenantStore tenantStore)
    {
        TenantStore = tenantStore;
    }

    public virtual async Task<string> CreateAsync(IdentityClient identityClient, CancellationToken cancellationToken = default)
    {
        if (identityClient.TenantId.HasValue)
        {
            var tenant = await TenantStore.FindAsync(identityClient.TenantId.Value);
            if (tenant != null)
            {
                return $"{tenant.Name}_{identityClient.Name}".ToLowerInvariant();
            }
        }

        return identityClient.Name.ToLowerInvariant();
    }
}
