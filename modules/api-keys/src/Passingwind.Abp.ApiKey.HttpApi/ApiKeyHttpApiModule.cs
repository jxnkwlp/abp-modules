using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.ApiKey.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(ApiKeyApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ApiKeyHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(ApiKeyHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ApiKeyResource>();
        });
    }
}
