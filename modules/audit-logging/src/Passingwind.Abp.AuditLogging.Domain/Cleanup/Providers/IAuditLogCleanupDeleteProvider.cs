using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.AuditLogging.Cleanup.Providers;

public interface IAuditLogCleanupDeleteProvider
{
    Task DeleteAsync(DateTime endTime, CancellationToken cancellationToken = default);
}
