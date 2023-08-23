using Passingwind.Abp.DynamicPermissionManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class DynamicPermissionManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DynamicPermissionManagementDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<DynamicPermissionManagementResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/DynamicPermissionManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("DynamicPermissionManagement", typeof(DynamicPermissionManagementResource)));
    }
}
