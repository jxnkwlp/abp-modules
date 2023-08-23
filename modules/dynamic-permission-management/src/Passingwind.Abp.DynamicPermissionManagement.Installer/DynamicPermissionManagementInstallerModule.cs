using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DynamicPermissionManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class DynamicPermissionManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DynamicPermissionManagementInstallerModule>());
    }
}
