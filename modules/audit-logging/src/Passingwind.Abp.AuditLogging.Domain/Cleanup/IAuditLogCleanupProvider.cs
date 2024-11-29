using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public interface IAuditLogCleanupProvider
{
    Task DeleteAsync(DateTime endTime, CancellationToken cancellationToken = default);
    Task BackupToFileAsync(DateTime endTime, CancellationToken cancellationToken = default);
}
