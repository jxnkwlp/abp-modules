using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.AuditLogging;

[Area(AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Route("api/audit-logging/audit-logs")]
public class AuditLogController : AbpControllerBase, IAuditLogAppService
{
    protected IAuditLogAppService Service { get; }

    public AuditLogController(IAuditLogAppService service)
    {
        Service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogListRequestDto input)
    {
        return Service.GetListAsync(input);
    }

    /// <inheritdoc/>
    [HttpGet("{id}")]
    public virtual Task<AuditLogDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    /// <inheritdoc/>
    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }
}
