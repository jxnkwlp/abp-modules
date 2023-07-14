using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class PassingwindAbpFileManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PassingwindAbpFileManagementInstallerModule>();
        });
    }
}
