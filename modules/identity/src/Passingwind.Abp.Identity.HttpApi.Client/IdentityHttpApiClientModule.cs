using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.Identity;
using Volo.Abp.Http.Client;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp;

[DependsOn(
    typeof(IdentityApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IdentityHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IdentityApplicationContractsModule).Assembly,
            IdentityRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityHttpApiClientModule>());
    }
}
