using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(ApiKeyApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ApiKeyHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ApiKeyApplicationContractsModule).Assembly,
            ApiKeyRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<ApiKeyHttpApiClientModule>());
    }
}
