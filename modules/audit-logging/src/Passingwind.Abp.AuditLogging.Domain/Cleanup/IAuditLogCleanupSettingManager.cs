using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public interface IAuditLogCleanupSettingManager
{
    Task<AuditLogCleanupSettings> GetAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(AuditLogCleanupSettings settings, CancellationToken cancellationToken = default);
}
