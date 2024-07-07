using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(PermissionManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class PermissionManagementApplicationContractsModule : AbpModule
{
}
