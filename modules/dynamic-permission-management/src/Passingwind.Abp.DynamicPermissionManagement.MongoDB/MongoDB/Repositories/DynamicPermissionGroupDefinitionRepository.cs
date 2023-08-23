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

public class DynamicPermissionGroupDefinitionRepository : MongoDbRepository<DynamicPermissionManagementMongoDbContext, DynamicPermissionGroupDefinition, Guid>, IDynamicPermissionGroupDefinitionRepository
{
    public DynamicPermissionGroupDefinitionRepository(IMongoDbContextProvider<DynamicPermissionManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
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
            .PageBy(skipCount, maxResultCount)
            .OrderBy(sorting ?? nameof(DynamicPermissionGroupDefinition.Name))
            .As<IMongoQueryable<DynamicPermissionGroupDefinition>>()
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
