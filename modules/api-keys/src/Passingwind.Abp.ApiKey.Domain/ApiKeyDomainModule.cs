using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(AbpIdentityDomainModule),
    typeof(ApiKeyDomainSharedModule)
)]
public class ApiKeyDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<AbpApiKeyOptions>("ApiKey");

        context.Services.AddEntityCache<ApiKeyRecord, Guid>(new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1),
            SlidingExpiration = TimeSpan.FromDays(1),
        });
    }
}
