using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(PassingwindAbpFileManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class PassingwindAbpFileManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PassingwindAbpFileManagementApplicationContractsModule).Assembly,
            FileManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PassingwindAbpFileManagementHttpApiClientModule>();
        });

    }
}
