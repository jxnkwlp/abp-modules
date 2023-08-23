using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(DynamicPermissionManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class DynamicPermissionManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(DynamicPermissionManagementApplicationContractsModule).Assembly,
            DynamicPermissionManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DynamicPermissionManagementHttpApiClientModule>());
    }
}
