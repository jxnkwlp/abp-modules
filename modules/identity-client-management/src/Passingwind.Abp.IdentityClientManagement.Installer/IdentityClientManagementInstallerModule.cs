using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.IdentityClientManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class IdentityClientManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityClientManagementInstallerModule>();
        });
    }
}
