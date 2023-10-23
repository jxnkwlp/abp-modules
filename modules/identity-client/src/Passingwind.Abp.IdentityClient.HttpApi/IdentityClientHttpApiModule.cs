using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.IdentityClient.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClient;

[DependsOn(
    typeof(IdentityClientApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class IdentityClientHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder => mvcBuilder.AddApplicationPartIfNotExists(typeof(IdentityClientHttpApiModule).Assembly));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityClientResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
