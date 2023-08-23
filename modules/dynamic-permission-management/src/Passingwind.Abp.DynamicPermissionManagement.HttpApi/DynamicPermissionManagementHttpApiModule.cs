using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.DynamicPermissionManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(DynamicPermissionManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class DynamicPermissionManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(DynamicPermissionManagementHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DynamicPermissionManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
