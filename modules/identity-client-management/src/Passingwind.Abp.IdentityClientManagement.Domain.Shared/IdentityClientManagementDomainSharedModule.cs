using Passingwind.Abp.IdentityClientManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class IdentityClientManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityClientManagementDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityClientManagementResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/IdentityClientManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("IdentityClientManagement", typeof(IdentityClientManagementResource)));
    }
}
