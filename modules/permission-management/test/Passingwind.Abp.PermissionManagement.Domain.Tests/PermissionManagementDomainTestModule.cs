using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(PermissionManagementDomainModule),
    typeof(PermissionManagementTestBaseModule)
)]
public class PermissionManagementDomainTestModule : AbpModule
{

}
