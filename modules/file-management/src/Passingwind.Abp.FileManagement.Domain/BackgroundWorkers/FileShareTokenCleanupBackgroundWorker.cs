using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Passingwind.Abp.FileManagement.BackgroundWorkers;

public class FileShareTokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    public FileShareTokenCleanupBackgroundWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        // 12h
        Timer.Period = 12 * 60 * 60 * 1000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Starting cleanup file share token");

        var manager = workerContext.ServiceProvider.GetRequiredService<FileAccessTokenManager>();

        await manager.DeleteAllExpirationTokenAsync(workerContext.CancellationToken);

        Logger.LogInformation("Cleanup file share token job done.");
    }
}
