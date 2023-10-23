using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.IdentityClient;

[DependsOn(
    typeof(IdentityClientApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IdentityClientHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IdentityClientApplicationContractsModule).Assembly,
            IdentityClientRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityClientHttpApiClientModule>());
    }
}
