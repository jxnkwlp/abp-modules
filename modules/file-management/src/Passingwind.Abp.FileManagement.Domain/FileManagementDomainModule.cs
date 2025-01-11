using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.FileManagement.BackgroundWorkers;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpBlobStoringModule),
    typeof(AbpBlobStoringFileSystemModule),
    typeof(FileManagementDomainSharedModule)
)]
public class FileManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<FileManagementOptions>("FileManagement");

        // Default configuration
        Configure<AbpBlobStoringOptions>(options => options.Containers.ConfigureDefault(container => container.UseFileSystem(fileSystem => fileSystem.BasePath = "./storage")));
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<FileShareTokenCleanupBackgroundWorker>();
    }
}
