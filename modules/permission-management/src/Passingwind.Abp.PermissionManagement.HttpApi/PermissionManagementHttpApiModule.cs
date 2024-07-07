using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.PermissionManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(PermissionManagementApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpAspNetCoreMvcModule))]
public class PermissionManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(PermissionManagementHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PermissionManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
