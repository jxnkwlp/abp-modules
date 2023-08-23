using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DictionaryManagement.MongoDB.Repositories;

public class DictionaryItemRepository : MongoDbRepository<DictionaryManagementMongoDbContext, DictionaryItem, Guid>, IDictionaryItemRepository
{
    public DictionaryItemRepository(IMongoDbContextProvider<DictionaryManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<DictionaryItem?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<DictionaryItem> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(DictionaryItem), name);

        return entity;
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .As<IMongoQueryable<DictionaryItem>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<DictionaryItem>> GetListAsync(string? filter = null, string? groupName = null, bool? isEnabled = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .OrderBy(sorting ?? nameof(DictionaryItem.Name))
            .As<IMongoQueryable<DictionaryItem>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<DictionaryItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? groupName = null, bool? isEnabled = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(groupName), x => x.GroupName == groupName)
            .WhereIf(isEnabled.HasValue, x => x.IsEnabled == isEnabled)
            .OrderBy(sorting ?? nameof(DictionaryItem.Name))
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<DictionaryItem>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string name, IEnumerable<Guid>? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .As<IMongoQueryable<DictionaryItem>>()
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
