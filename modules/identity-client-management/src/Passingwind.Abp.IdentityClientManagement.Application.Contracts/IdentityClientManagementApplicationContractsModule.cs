using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class IdentityClientManagementApplicationContractsModule : AbpModule
{
}
