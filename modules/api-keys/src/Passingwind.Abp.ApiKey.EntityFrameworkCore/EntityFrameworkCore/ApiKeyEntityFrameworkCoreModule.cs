using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.ApiKey.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey.EntityFrameworkCore;

[DependsOn(
    typeof(ApiKeyDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ApiKeyEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ApiKeyDbContext>(options => options.AddRepository<ApiKeyRecord, ApiKeyRecordRepository>());
    }
}
