using Volo.Abp.Modularity;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(DynamicPermissionManagementApplicationModule),
    typeof(DynamicPermissionManagementDomainTestModule)
    )]
public class DynamicPermissionManagementApplicationTestModule : AbpModule
{
}
