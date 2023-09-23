using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.ApiKey;

public interface IApiKeyRecordManager : IDomainService
{
    Task<ApiKeyCacheItem> FindFromCacheAsync(string value, CancellationToken cancellationToken = default);

    Task<ApiKeyRecord> RegenerateValueAsync(ApiKeyRecord record, CancellationToken cancellationToken = default);

    Task<string> GenerateValueAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IList<Claim>?> GetClaimsAsync(ApiKeyRecord record, CancellationToken cancellationToken = default);

    Task<ApiKeyRecord> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
