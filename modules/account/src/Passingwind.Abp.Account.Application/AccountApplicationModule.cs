using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.Identity;
using Passingwind.Abp.Identity.AspNetCore;
using Volo.Abp.Account;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AccountDomainModule),
    typeof(AccountApplicationContractsModule),
    typeof(IdentityAspNetCoreModule),
    typeof(IdentityDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class AccountApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AccountApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<AccountApplicationModule>(validate: true));
    }
}
