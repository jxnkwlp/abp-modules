using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.PermissionManagement.EntityFrameworkCore.Repositories;

public class DynamicPermissionGroupDefinitionRepository : EfCoreRepository<PermissionManagementDbContextV2, DynamicPermissionGroupDefinition, Guid>, IDynamicPermissionGroupDefinitionRepository
{
    public DynamicPermissionGroupDefinitionRepository(IDbContextProvider<PermissionManagementDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionGroupDefinition>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionGroupDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .OrderBy(sorting ?? nameof(DynamicPermissionGroupDefinition.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .Where(x => x.Name == name)
            .AnyAsync(cancellationToken);
    }
}
