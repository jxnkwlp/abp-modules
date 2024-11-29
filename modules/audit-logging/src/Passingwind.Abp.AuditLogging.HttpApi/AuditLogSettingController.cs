using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.AuditLogging;

[Area(AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Route("api/audit-logging/settings")]
public class AuditLogSettingController : AbpControllerBase, IAuditLogSettingAppService
{
    private readonly IAuditLogSettingAppService _service;

    public AuditLogSettingController(IAuditLogSettingAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet("cleanup")]
    public virtual Task<AuditLogCleanupSettingsDto> GetCleanupAsync()
    {
        return _service.GetCleanupAsync();
    }

    /// <inheritdoc/>
    [HttpPut("cleanup")]
    public virtual Task UpdateCleanupAsync(AuditLogCleanupSettingsDto input)
    {
        return _service.UpdateCleanupAsync(input);
    }
}
