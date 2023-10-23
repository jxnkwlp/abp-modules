using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClient.MongoDB.Repositories;

public class IdentityClientRepository : MongoDbRepository<IdentityClientMongoDbContext, IdentityClient, Guid>, IIdentityClientRepository
{
    public IdentityClientRepository(IMongoDbContextProvider<IdentityClientMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<IdentityClient?> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
           .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<IdentityClient?> FindByProviderNameAsync(string providerName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();
        return await query
           .FirstOrDefaultAsync(x => x.ProviderName == providerName, cancellationToken: cancellationToken);
    }

    public async Task<IdentityClient> GetByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);

        if (entity == null)
            throw new EntityNotFoundException(typeof(IdentityClient));

        return entity;
    }

    public virtual async Task<long> GetCountAsync(string? filter, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .As<IMongoQueryable<IdentityClient>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityClient>> GetListAsync(string? filter, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .As<IMongoQueryable<IdentityClient>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<IdentityClient>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!) || x.DisplayName.Contains(filter!))
            .OrderBy(sorting ?? nameof(IdentityClient.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<IdentityClient>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameExistsAsync(string name, Guid[]? excludeIds = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(excludeIds != null, x => !excludeIds!.Contains(x.Id))
            .As<IMongoQueryable<IdentityClient>>()
            .AnyAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }
}
