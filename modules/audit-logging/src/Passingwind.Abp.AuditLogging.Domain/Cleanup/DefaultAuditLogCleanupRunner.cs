using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public class DefaultAuditLogCleanupRunner : IAuditLogCleanupRunner, IScopedDependency
{
    protected ILogger<DefaultAuditLogCleanupRunner> Logger { get; }
    protected IClock Clock { get; }
    protected IAuditLogCleanupProvider CleanupProvider { get; }
    protected IAuditLogCleanupSettingManager CleanupSettingManager { get; }

    public DefaultAuditLogCleanupRunner(
        ILogger<DefaultAuditLogCleanupRunner> logger,
        IClock clock,
        IAuditLogCleanupProvider cleanupProvider,
        IAuditLogCleanupSettingManager cleanupSettingManager)
    {
        Logger = logger;
        Clock = clock;
        CleanupProvider = cleanupProvider;
        CleanupSettingManager = cleanupSettingManager;
    }

    public virtual async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var settings = await CleanupSettingManager.GetAsync(cancellationToken);

        var behavior = settings.Behavior;

        if (behavior == AuditLogCleanupBehavior.None)
        {
            return;
        }

        var endTime = Clock.Now.AddDays(-settings.KeepDays);

        await RunAsync(behavior, endTime, cancellationToken);
    }

    public virtual async Task RunAsync(AuditLogCleanupBehavior behavior, DateTime endTime, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Start cleanup audit logs...");

        switch (behavior)
        {
            case AuditLogCleanupBehavior.Delete:
                await CleanupProvider.DeleteAsync(endTime, cancellationToken);
                break;
            case AuditLogCleanupBehavior.BackupToFile:
                await CleanupProvider.BackupToFileAsync(endTime, cancellationToken);
                break;
        }

        Logger.LogInformation("Audit logs cleaned up.");
    }
}
