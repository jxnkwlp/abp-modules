using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.IdentityClient;

[DependsOn(
    typeof(IdentityClientDomainModule),
    typeof(IdentityClientApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class IdentityClientApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<IdentityClientApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<IdentityClientApplicationModule>(validate: true));
    }
}
