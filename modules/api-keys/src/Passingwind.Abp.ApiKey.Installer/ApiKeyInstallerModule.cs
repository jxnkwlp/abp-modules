using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Passingwind.Abp.ApiKey;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ApiKeyInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ApiKeyInstallerModule>();
        });
    }
}
