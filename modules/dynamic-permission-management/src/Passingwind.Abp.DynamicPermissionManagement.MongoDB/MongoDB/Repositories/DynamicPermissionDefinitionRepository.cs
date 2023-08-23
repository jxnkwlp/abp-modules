using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB.Repositories;

public class DynamicPermissionDefinitionRepository : MongoDbRepository<DynamicPermissionManagementMongoDbContext, DynamicPermissionDefinition, Guid>, IDynamicPermissionDefinitionRepository
{
    public DynamicPermissionDefinitionRepository(IMongoDbContextProvider<DynamicPermissionManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
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
            .PageBy(skipCount, maxResultCount)
            .OrderBy(sorting ?? nameof(DynamicPermissionDefinition.Name))
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .Where(x => x.Name == name)
            .As<IMongoQueryable<DynamicPermissionDefinition>>()
            .AnyAsync(cancellationToken);
    }
}
