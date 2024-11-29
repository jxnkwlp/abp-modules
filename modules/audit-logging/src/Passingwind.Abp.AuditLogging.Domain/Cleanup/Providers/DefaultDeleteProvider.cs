using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.AuditLogging.Cleanup.Providers;

[ExposeServices(typeof(IAuditLogCleanupDeleteProvider))]
public class DefaultDeleteProvider : IAuditLogCleanupDeleteProvider, IScopedDependency
{
    protected ILogger<DefaultDeleteProvider> Logger { get; }
    protected IAuditLogRepository AuditLogRepository { get; }

    public DefaultDeleteProvider(ILogger<DefaultDeleteProvider> logger, IAuditLogRepository auditLogRepository)
    {
        Logger = logger;
        AuditLogRepository = auditLogRepository;
    }

    public virtual async Task DeleteAsync(DateTime endTime, CancellationToken cancellationToken = default)
    {
        await AuditLogRepository.DeleteDirectAsync(x => x.ExecutionTime < endTime, cancellationToken);
    }
}
