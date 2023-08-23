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

public class DictionaryGroupRepository : MongoDbRepository<DictionaryManagementMongoDbContext, DictionaryGroup, Guid>, IDictionaryGroupRepository
{
    public DictionaryGroupRepository(IMongoDbContextProvider<DictionaryManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<DictionaryGroup?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<DictionaryGroup> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(DictionaryGroup), name);

        return entity;
    }

    public async Task<long> GetCountAsync(string? filter = null, string? parentName = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .As<IMongoQueryable<DictionaryGroup>>()
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<DictionaryGroup>> GetListAsync(string? filter = null, string? parentName = null, bool includeDetails = false, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .OrderBy(sorting ?? nameof(DictionaryGroup.Name))
            .As<IMongoQueryable<DictionaryGroup>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DictionaryGroup>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, string? parentName = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentName == parentName)
            .OrderBy(sorting ?? nameof(DictionaryGroup.Name))
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<DictionaryGroup>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string name, IEnumerable<Guid>? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds?.Any() == true, x => !excludeIds!.Contains(x.Id))
            .As<IMongoQueryable<DictionaryGroup>>()
            .AnyAsync(x => x.Name == name, cancellationToken);
    }
}
