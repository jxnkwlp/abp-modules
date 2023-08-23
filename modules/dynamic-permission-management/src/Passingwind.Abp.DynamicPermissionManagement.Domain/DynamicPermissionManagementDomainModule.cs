using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpPermissionManagementDomainModule),
    typeof(DynamicPermissionManagementDomainSharedModule)
)]
public class DynamicPermissionManagementDomainModule : AbpModule
{
}
