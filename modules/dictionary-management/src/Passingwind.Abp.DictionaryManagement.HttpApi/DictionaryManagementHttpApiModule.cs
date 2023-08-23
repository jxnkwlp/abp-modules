using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.DictionaryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(DictionaryManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class DictionaryManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(DictionaryManagementHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DictionaryManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
