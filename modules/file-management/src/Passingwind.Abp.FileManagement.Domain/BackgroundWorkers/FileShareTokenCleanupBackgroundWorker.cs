using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Passingwind.Abp.FileManagement.BackgroundWorkers;

public class FileShareTokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    public FileShareTokenCleanupBackgroundWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        // 6h
        Timer.Period = 6 * 60 * 60 * 1000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Starting cleanup file share token");

        var manager = workerContext.ServiceProvider.GetRequiredService<FileAccessTokenManager>();

        await manager.DeleteAllExpirationTokenAsync(workerContext.CancellationToken);

        Logger.LogInformation("Cleanup file share token job done.");
    }
}
