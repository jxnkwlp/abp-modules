using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(PassingwindAbpFileManagementDomainModule),
    typeof(PassingwindAbpFileManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class PassingwindAbpFileManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PassingwindAbpFileManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PassingwindAbpFileManagementApplicationModule>(validate: true);
        });
    }
}
