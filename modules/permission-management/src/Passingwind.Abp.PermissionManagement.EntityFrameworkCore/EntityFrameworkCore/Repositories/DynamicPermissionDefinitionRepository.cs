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

public class DynamicPermissionDefinitionRepository : EfCoreRepository<PermissionManagementDbContextV2, DynamicPermissionDefinition, Guid>, IDynamicPermissionDefinitionRepository
{
    public DynamicPermissionDefinitionRepository(IDbContextProvider<PermissionManagementDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionDefinition>> GetListAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? groupId = null, Guid? parentId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .OrderBy(sorting ?? nameof(DynamicPermissionDefinition.Name))
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
