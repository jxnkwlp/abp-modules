using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.AuditLogging.Cleanup;
using Passingwind.Abp.AuditLogging.Permissions;

namespace Passingwind.Abp.AuditLogging;

[Authorize(AuditLoggingPermissions.AuditLogs.Delete)]
public class AuditLogCleanupAppService : AuditLoggingAppServiceBase, IAuditLogCleanupAppService
{
    protected IAuditLogCleanupRunner AuditLogCleanupRunner { get; }

    public AuditLogCleanupAppService(IAuditLogCleanupRunner auditLogCleanupRunner)
    {
        AuditLogCleanupRunner = auditLogCleanupRunner;
    }

    public virtual async Task RunAsync(AuditLogCleanupRequestDto input)
    {
        await AuditLogCleanupRunner.RunAsync(input.Behavior, input.EndTime.Date);
    }
}
