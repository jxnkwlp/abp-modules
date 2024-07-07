using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Passingwind.Abp.PermissionManagement.MongoDB;

[DependsOn(
    typeof(PermissionManagementDomainModule),
    typeof(PermissionManagementDomainModule),
    typeof(AbpPermissionManagementMongoDbModule)
    )]
public class PermissionManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<PermissionManagementMongoDbContextV2>(options => options.AddDefaultRepositories());
    }
}
