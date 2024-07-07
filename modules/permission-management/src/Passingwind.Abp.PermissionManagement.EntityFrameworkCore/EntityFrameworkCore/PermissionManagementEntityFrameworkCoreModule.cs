using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Passingwind.Abp.PermissionManagement.EntityFrameworkCore;

[DependsOn(
    typeof(PermissionManagementDomainModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule)
)]
public class PermissionManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PermissionManagementDbContextV2>(options => options.AddDefaultRepositories());
    }
}
