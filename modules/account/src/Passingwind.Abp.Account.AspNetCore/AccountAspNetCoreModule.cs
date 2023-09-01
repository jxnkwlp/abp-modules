using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainModule),
    typeof(AccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAutoMapperModule)
    )]
public class AccountAspNetCoreModule : AbpModule
{
}
