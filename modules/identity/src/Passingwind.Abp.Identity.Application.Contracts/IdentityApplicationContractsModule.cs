using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Identity;

[DependsOn(
    typeof(IdentityDomainSharedModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
)]
public class IdentityApplicationContractsModule : AbpModule
{
}
