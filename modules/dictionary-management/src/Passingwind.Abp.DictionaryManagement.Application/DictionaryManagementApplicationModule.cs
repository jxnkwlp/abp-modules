using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(DictionaryManagementDomainModule),
    typeof(DictionaryManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class DictionaryManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<DictionaryManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<DictionaryManagementApplicationModule>(validate: true));
    }
}
