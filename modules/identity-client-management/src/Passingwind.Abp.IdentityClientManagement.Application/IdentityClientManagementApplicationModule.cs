using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementDomainModule),
    typeof(IdentityClientManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class IdentityClientManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityClientManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<IdentityClientManagementApplicationModule>(validate: true));
    }
}
