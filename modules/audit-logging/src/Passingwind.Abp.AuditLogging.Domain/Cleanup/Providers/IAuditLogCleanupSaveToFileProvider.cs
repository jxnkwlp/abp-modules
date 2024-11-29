using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.AuditLogging.Cleanup.Providers;

public interface IAuditLogCleanupSaveToFileProvider
{
    Task SaveToFileAsync(DateTime endTime, CancellationToken cancellationToken = default);
}
