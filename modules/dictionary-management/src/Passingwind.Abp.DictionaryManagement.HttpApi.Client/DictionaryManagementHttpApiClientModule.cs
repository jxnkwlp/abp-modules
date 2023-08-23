using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(DictionaryManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class DictionaryManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(DictionaryManagementApplicationContractsModule).Assembly,
            DictionaryManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DictionaryManagementHttpApiClientModule>());
    }
}
