using Passingwind.Abp.Identity.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.Identity;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpIdentityDomainSharedModule)
)]
public class IdentityDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityResourceV2>("en")
                .AddBaseTypes(typeof(IdentityResource))
                .AddVirtualJson("/Localization/IdentityV2");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("Identity", typeof(IdentityResourceV2)));
    }
}
