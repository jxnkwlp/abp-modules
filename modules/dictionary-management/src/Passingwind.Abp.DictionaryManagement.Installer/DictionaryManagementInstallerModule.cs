using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.DictionaryManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class DictionaryManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<DictionaryManagementInstallerModule>());
    }
}
