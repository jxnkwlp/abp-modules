using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.AuditLogging;

[DependsOn(
    typeof(AuditLoggingDomainModule),
    typeof(AuditLoggingApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class AuditLoggingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AuditLoggingApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => options.AddMaps<AuditLoggingApplicationModule>(validate: true));
    }
}
