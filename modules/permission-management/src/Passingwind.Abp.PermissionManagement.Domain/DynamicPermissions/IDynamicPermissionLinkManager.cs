using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public interface IDynamicPermissionLinkManager
{
    Task AddOrUpdateAsync(string dynamicPermissionName, IEnumerable<string> targetNames, CancellationToken cancellationToken = default);
    Task DeleteAsync(string dynamicPermissionName, CancellationToken cancellationToken = default);
    Task<List<string>> GetLinksAsync(string dynamicPermissionName, CancellationToken cancellationToken = default);
}
