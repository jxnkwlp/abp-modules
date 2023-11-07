using Passingwind.Abp.Identity;
using Volo.Abp.Account;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainSharedModule),
    typeof(IdentityApplicationContractsModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AccountApplicationContractsModule : AbpModule
{
}
