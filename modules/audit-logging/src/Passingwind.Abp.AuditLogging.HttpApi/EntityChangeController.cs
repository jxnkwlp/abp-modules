using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Passingwind.Abp.AuditLogging;

[Area(AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Route("api/audit-logging/entity-changes")]
public class EntityChangeController : AbpControllerBase, IEntityChangeAppService
{
    protected IEntityChangeAppService Service { get; }

    public EntityChangeController(IEntityChangeAppService service)
    {
        Service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<ListResultDto<EntityChangeDto>> GetListAsync(EntityChangeListRequestDto input)
    {
        return Service.GetListAsync(input);
    }

    /// <inheritdoc/>
    [HttpGet("{id}")]
    public virtual Task<EntityChangeDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }
}
