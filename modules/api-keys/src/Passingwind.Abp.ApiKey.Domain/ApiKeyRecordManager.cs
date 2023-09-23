using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyRecordManager : DomainService, IApiKeyRecordManager
{
    private readonly IApiKeyRecordRepository _apiKeyRecordRepository;
    private readonly IEntityCache<ApiKeyRecord, Guid> _apiKeyEntityCache;
    private readonly IDistributedCache<ApiKeyCacheItem> _distributedCache;
    private readonly IdentityUserManager _userManager;

    public ApiKeyRecordManager(IApiKeyRecordRepository apiKeyRecordRepository, IEntityCache<ApiKeyRecord, Guid> apiKeyEntityCache, IDistributedCache<ApiKeyCacheItem> distributedCache, IdentityUserManager userManager)
    {
        _apiKeyRecordRepository = apiKeyRecordRepository;
        _apiKeyEntityCache = apiKeyEntityCache;
        _distributedCache = distributedCache;
        _userManager = userManager;
    }

    [UnitOfWork]
    public virtual async Task<ApiKeyRecord> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _apiKeyEntityCache.FindAsync(id);
    }

    [UnitOfWork]
    public virtual async Task<ApiKeyCacheItem> FindFromCacheAsync(string value, CancellationToken cancellationToken = default)
    {
        return await _distributedCache.GetOrAddAsync(value, async () =>
        {
            var entity = await _apiKeyRecordRepository.FindBySecretAsync(value);
            return entity == null ? ApiKeyCacheItem.Empty() : new ApiKeyCacheItem(entity.Id, entity.UserId, entity.Secret, entity.ExpirationTime);
        }, () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1),
        }, token: cancellationToken);
    }

    public virtual Task<string> GenerateValueAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.NewGuid().ToString("N"));
    }

    [UnitOfWork]
    public virtual async Task<IList<Claim>?> GetClaimsAsync(ApiKeyRecord record, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(record.UserId.ToString());

        if (user == null)
            return null;

        return await _userManager.GetClaimsAsync(user);
    }

    [UnitOfWork]
    public virtual async Task<ApiKeyRecord> RegenerateValueAsync(ApiKeyRecord record, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetByIdAsync(record.UserId);

        record.Secret = await GenerateValueAsync(user.Id);

        return await _apiKeyRecordRepository.UpdateAsync(record);
    }
}
