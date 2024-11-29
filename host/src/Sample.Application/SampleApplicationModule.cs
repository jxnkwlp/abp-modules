using Passingwind.Abp.Account;
using Passingwind.Abp.ApiKey;
using Passingwind.Abp.AuditLogging;
using Passingwind.Abp.FileManagement;
using Passingwind.Abp.Identity;
using Passingwind.Abp.IdentityClient;
using Passingwind.Abp.PermissionManagement;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Sample;

[DependsOn(
    typeof(SampleDomainModule),
    typeof(SampleApplicationContractsModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
[DependsOn(typeof(AccountApplicationModule))]
[DependsOn(typeof(ApiKeyApplicationModule))]
[DependsOn(typeof(FileManagementApplicationModule))]
[DependsOn(typeof(IdentityApplicationModule))]
[DependsOn(typeof(IdentityClientApplicationModule))]
[DependsOn(typeof(PermissionManagementApplicationModule))]
[DependsOn(typeof(AuditLoggingApplicationModule))]
public class SampleApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<SampleApplicationModule>());
    }
}
