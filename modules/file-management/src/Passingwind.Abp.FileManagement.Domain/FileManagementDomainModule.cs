﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Abp.FileManagement.BackgroundWorkers;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Passingwind.Abp.FileManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(FileManagementDomainSharedModule),
    typeof(AbpBlobStoringModule)
)]
public class FileManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<FileManagementOptions>("FileManagement");

        context.Services.AddTransient<IFileDuplicateDetectionProvider, FileNameDuplicateDetectionProvider>();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<FileShareTokenCleanupBackgroundWorker>();
    }
}
