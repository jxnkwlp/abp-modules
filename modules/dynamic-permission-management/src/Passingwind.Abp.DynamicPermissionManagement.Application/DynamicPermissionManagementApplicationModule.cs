using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(DynamicPermissionManagementDomainModule),
    typeof(DynamicPermissionManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class DynamicPermissionManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<DynamicPermissionManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<DynamicPermissionManagementApplicationModule>(validate: true));
    }
}
