using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.ApiKey;

public interface IApiKeyRecordRepository : IRepository<ApiKeyRecord, Guid>
{
    Task<long> GetCountAsync(string? filter = null, Guid? userId = null, CancellationToken cancellationToken = default);

    Task<List<ApiKeyRecord>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<List<ApiKeyRecord>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<ApiKeyRecord?> FindBySecretAsync(string value, CancellationToken cancellationToken = default);
}
