using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class PermissionManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PermissionManagementInstallerModule>();
        });
    }
}
