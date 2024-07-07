using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(PermissionManagementApplicationModule),
    typeof(PermissionManagementDomainTestModule)
    )]
public class PermissionManagementApplicationTestModule : AbpModule
{

}
