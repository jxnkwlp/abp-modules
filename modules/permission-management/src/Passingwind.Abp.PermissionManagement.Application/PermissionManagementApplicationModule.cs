using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(PermissionManagementDomainModule),
    typeof(PermissionManagementApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpPermissionManagementApplicationModule)
    )]
public class PermissionManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PermissionManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<PermissionManagementApplicationModule>(validate: true));
    }
}
