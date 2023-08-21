using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClientManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class IdentityClientManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(IdentityClientManagementHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityClientManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
