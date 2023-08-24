using Passingwind.Abp.DictionaryManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class DictionaryManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DictionaryManagementDomainSharedModule>());

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<DictionaryManagementResource>("en")
                .AddVirtualJson("/Localization/DictionaryManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options => options.MapCodeNamespace("DictionaryManagement", typeof(DictionaryManagementResource)));
    }
}
