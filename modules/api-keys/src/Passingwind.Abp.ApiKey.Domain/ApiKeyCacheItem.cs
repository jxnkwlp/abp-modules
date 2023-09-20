using System;
using Volo.Abp.Caching;

namespace Passingwind.Abp.ApiKey;

[CacheName("ApiKey")]
[Serializable]
public class ApiKeyCacheItem
{
    public Guid? CacheId { get; set; }
    public Guid UserId { get; set; }
    public string Secret { get; set; } = null!;
    public DateTime? ExpirationTime { get; set; }

    public static ApiKeyCacheItem Empty() => new ApiKeyCacheItem();

    public ApiKeyCacheItem()
    {
    }

    public ApiKeyCacheItem(Guid cacheId, Guid userId, string secret, DateTime? expirationTime)
    {
        CacheId = cacheId;
        UserId = userId;
        Secret = secret;
        ExpirationTime = expirationTime;
    }

    public override string ToString()
    {
        return $"CacheId: {CacheId}";
    }
}
