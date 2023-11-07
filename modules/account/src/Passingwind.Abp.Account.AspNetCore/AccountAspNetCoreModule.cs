using Passingwind.Abp.Identity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainModule),
    typeof(AccountApplicationContractsModule),
    typeof(IdentityDomainModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpIdentityAspNetCoreModule)
)]
public class AccountAspNetCoreModule : AbpModule
{
}
