using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainModule),
    typeof(AccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule)
    )]
public class AccountAspNetCoreModule : AbpModule
{
}
