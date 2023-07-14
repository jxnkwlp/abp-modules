using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.FileManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(PassingwindAbpFileManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class PassingwindAbpFileManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PassingwindAbpFileManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<FileManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
