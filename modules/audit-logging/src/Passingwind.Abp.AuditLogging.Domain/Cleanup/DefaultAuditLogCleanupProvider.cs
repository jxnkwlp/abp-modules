using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.AuditLogging.Cleanup.Providers;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public class DefaultAuditLogCleanupProvider : IAuditLogCleanupProvider, IScopedDependency
{
    protected ILogger<DefaultAuditLogCleanupProvider> Logger { get; }
    protected IAuditLogCleanupSaveToFileProvider SaveToFileProvider { get; }
    protected IAuditLogCleanupDeleteProvider DeleteProvider { get; }

    public DefaultAuditLogCleanupProvider(
        ILogger<DefaultAuditLogCleanupProvider> logger,
        IAuditLogCleanupSaveToFileProvider saveToFileProvider,
        IAuditLogCleanupDeleteProvider deleteProvider)
    {
        Logger = logger;
        SaveToFileProvider = saveToFileProvider;
        DeleteProvider = deleteProvider;
    }

    public virtual async Task DeleteAsync(DateTime endTime, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Start cleanup audit logs by delete and ends to '{endTime}'", endTime);
        await DeleteProvider.DeleteAsync(endTime, cancellationToken);
    }

    public virtual async Task BackupToFileAsync(DateTime endTime, CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Start cleanup audit logs by save to file and ends to '{endTime}'", endTime);
        await SaveToFileProvider.SaveToFileAsync(endTime, cancellationToken);
    }
}
