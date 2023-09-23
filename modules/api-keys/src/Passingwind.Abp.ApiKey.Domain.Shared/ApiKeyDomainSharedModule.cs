using Passingwind.Abp.ApiKey.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class ApiKeyDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<ApiKeyDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ApiKeyResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/ApiKey");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("ApiKey", typeof(ApiKeyResource)));
    }
}
