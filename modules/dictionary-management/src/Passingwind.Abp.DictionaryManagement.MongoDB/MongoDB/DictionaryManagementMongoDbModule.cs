using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DictionaryManagement.MongoDB;

[DependsOn(
    typeof(DictionaryManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class DictionaryManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<DictionaryManagementMongoDbContext>(options => options.AddDefaultRepositories());
    }
}
