using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpPermissionManagementDomainModule),
    typeof(PermissionManagementDomainSharedModule)
)]
public class PermissionManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options => options.IsDynamicPermissionStoreEnabled = true);
    }
}
