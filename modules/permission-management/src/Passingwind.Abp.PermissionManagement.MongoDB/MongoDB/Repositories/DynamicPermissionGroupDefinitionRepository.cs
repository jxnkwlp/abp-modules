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

public class DynamicPermissionGroupDefinitionRepository : MongoDbRepository<PermissionManagementMongoDbContextV2, DynamicPermissionGroupDefinition, Guid>, IDynamicPermissionGroupDefinitionRepository
{
    public DynamicPermissionGroupDefinitionRepository(IMongoDbContextProvider<PermissionManagementMongoDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionGroupDefinition>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DynamicPermissionGroupDefinition>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .OrderBy(sorting ?? nameof(DynamicPermissionGroupDefinition.Name))
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .Where(x => x.Name == name)
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .AnyAsync(cancellationToken);
    }
}
