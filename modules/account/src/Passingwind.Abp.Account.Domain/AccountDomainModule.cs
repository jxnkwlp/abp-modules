using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Account;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AccountDomainSharedModule)
)]
public class AccountDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<AccountExternalLoginOptions>("AccountExternal");
    }
}
