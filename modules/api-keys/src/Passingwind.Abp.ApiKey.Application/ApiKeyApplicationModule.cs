using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(ApiKeyDomainModule),
    typeof(ApiKeyApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class ApiKeyApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ApiKeyApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<ApiKeyApplicationModule>(validate: true));
    }
}
