using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.ApiKey.EntityFrameworkCore.Repositories;

public class ApiKeyRecordRepository : EfCoreRepository<ApiKeyDbContext, ApiKeyRecord, Guid>, IApiKeyRecordRepository
{
    public ApiKeyRecordRepository(IDbContextProvider<ApiKeyDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<ApiKeyRecord?> FindBySecretAsync(string value, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .FirstOrDefaultAsync(x => x.Secret == value, cancellationToken);
    }

    public async Task<long> GetCountAsync(string? filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<ApiKeyRecord>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ApiKeyRecord>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter!))
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .OrderBy(sorting ?? nameof(ApiKeyRecord.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }
}
