using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.ApiKey.MongoDB.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.ApiKey.MongoDB;

[DependsOn(
    typeof(ApiKeyDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class ApiKeyMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<ApiKeyMongoDbContext>(options => options.AddRepository<ApiKeyRecord, ApiKeyRecordRepository>());
    }
}
