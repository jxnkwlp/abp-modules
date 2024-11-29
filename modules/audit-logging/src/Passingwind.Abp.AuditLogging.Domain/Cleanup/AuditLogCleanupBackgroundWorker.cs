using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public class AuditLogCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    public AuditLogCleanupBackgroundWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 24 * 60 * 60 * 1000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        try
        {
            var service = workerContext.ServiceProvider.GetRequiredService<IAuditLogCleanupRunner>();

            await service.RunAsync(workerContext.CancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Run audit log cleanup job failed.");
        }
    }
}
