using Passingwind.Abp.PermissionManagement.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class PermissionManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<PermissionManagementDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PermissionManagementResource>("en")
                .AddVirtualJson("/Localization/PermissionManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("PermissionManagement", typeof(PermissionManagementResource)));
    }
}
