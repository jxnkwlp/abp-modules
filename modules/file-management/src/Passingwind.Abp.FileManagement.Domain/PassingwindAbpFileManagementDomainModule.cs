using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PassingwindAbpFileManagementDomainSharedModule),
    typeof(AbpBlobStoringModule)
)]
public class PassingwindAbpFileManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<FileManagementOptions>();

        context.Services.AddTransient<IFileDuplicateDetectionProvider, FileNameDuplicateDetectionProvider>();
    }
}
