using Localization.Resources.AbpUi;
using Passingwind.Abp.Account;
using Passingwind.Abp.ApiKey;
using Passingwind.Abp.AuditLogging;
using Passingwind.Abp.FileManagement;
using Passingwind.Abp.Identity;
using Passingwind.Abp.IdentityClient;
using Sample.Localization;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Sample;

[DependsOn(
    typeof(SampleApplicationContractsModule),
    typeof(AccountHttpApiModule),
    typeof(IdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
[DependsOn(typeof(ApiKeyHttpApiModule))]
[DependsOn(typeof(FileManagementHttpApiModule))]
[DependsOn(typeof(AbpBlobStoringFileSystemModule))]
[DependsOn(typeof(IdentityClientHttpApiModule))]
[DependsOn(typeof(AuditLoggingHttpApiModule))]
public class SampleHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SampleResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
