using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(IdentityClientManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class IdentityClientManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(IdentityClientManagementApplicationContractsModule).Assembly,
            IdentityClientManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<IdentityClientManagementHttpApiClientModule>());
    }
}
