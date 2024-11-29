using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.AuditLogging;

[Area(AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Route("api/audit-logging/audit-logs")]
public class AuditLogCleanupController : AbpControllerBase, IAuditLogCleanupAppService
{
    protected IAuditLogCleanupAppService Service { get; }

    public AuditLogCleanupController(IAuditLogCleanupAppService service)
    {
        Service = service;
    }

    /// <inheritdoc/>
    [HttpPost("cleanup")]
    public virtual Task RunAsync(AuditLogCleanupRequestDto input)
    {
        return Service.RunAsync(input);
    }
}
