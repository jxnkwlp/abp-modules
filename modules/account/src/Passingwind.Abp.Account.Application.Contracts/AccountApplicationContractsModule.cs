using Volo.Abp.Account;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AccountApplicationContractsModule : AbpModule
{
}
