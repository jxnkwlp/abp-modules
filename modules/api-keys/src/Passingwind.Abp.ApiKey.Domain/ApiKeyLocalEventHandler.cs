using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyLocalEventHandler : ILocalEventHandler<EntityDeletedEventData<ApiKeyRecord>>, ITransientDependency
{
    private readonly IDistributedCache<ApiKeyCacheItem> _distributedCache;

    public ApiKeyLocalEventHandler(IDistributedCache<ApiKeyCacheItem> distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public virtual async Task HandleEventAsync(EntityDeletedEventData<ApiKeyRecord> eventData)
    {
        await _distributedCache.RemoveAsync(eventData.Entity.Secret);
    }
}
