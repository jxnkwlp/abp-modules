using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(DynamicPermissionManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class DynamicPermissionManagementApplicationContractsModule : AbpModule
{
}
