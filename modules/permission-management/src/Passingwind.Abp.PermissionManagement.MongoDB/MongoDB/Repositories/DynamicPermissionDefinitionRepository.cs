using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.PermissionManagement.MongoDB.Repositories;

public class DynamicPermissionDefinitionRepository : MongoDbRepository<PermissionManagementMongoDbContextV2, DynamicPermissionDefinition, Guid>, IDynamicPermissionDefinitionRepository
{
    public DynamicPermissionDefinitionRepository(IMongoDbContextProvider<PermissionManagementMongoDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionDefinition>> GetListAsync(string? filter = null, Guid? groupId = null, Guid? parentId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? groupId = null, Guid? parentId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .WhereIf(groupId.HasValue, x => x.GroupId == groupId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .OrderBy(sorting ?? nameof(DynamicPermissionDefinition.Name))
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .Where(x => x.Name == name)
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .AnyAsync(cancellationToken);
    }
}
