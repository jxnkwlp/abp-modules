using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.ApiKey.MongoDB.Repositories;

public class ApiKeyRecordRepository : MongoDbRepository<ApiKeyMongoDbContext, ApiKeyRecord, Guid>, IApiKeyRecordRepository
{
    public ApiKeyRecordRepository(IMongoDbContextProvider<ApiKeyMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<ApiKeyRecord?> FindBySecretAsync(string value, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .FirstOrDefaultAsync(x => x.Secret == value, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .As<IMongoQueryable<ApiKeyRecord>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<ApiKeyRecord>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .As<IMongoQueryable<ApiKeyRecord>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ApiKeyRecord>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .OrderBy(sorting ?? nameof(ApiKeyRecord.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<ApiKeyRecord>>()
            .ToListAsync(cancellationToken);
    }
}
