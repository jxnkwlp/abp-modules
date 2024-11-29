using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.AuditLogging;

[DependsOn(
    typeof(AuditLoggingApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AuditLoggingHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AuditLoggingApplicationContractsModule).Assembly,
            AuditLoggingRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<AuditLoggingHttpApiClientModule>());
    }
}
