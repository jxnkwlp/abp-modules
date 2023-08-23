using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public interface IDynamicPermissionDefinitionRepository : IRepository<DynamicPermissionDefinition, Guid>
{
    Task<long> GetCountAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<List<DynamicPermissionDefinition>> GetListAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<DynamicPermissionDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? groupId = null, Guid? parentId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default);
}
