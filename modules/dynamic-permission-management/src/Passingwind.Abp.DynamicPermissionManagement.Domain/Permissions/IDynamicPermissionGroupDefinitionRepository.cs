using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public interface IDynamicPermissionGroupDefinitionRepository : IRepository<DynamicPermissionGroupDefinition, Guid>
{
    Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default);

    Task<List<DynamicPermissionGroupDefinition>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<DynamicPermissionGroupDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default);
}
