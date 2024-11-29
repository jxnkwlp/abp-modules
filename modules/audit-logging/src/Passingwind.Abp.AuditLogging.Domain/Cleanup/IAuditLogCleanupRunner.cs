using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public interface IAuditLogCleanupRunner
{
    Task RunAsync(CancellationToken cancellationToken = default);
    Task RunAsync(AuditLogCleanupBehavior behavior, DateTime endTime, CancellationToken cancellationToken = default);
}
