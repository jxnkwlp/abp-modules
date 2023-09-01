using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/security-logs")]
public class IdentitySecurityLogController : IdentityBaseController, IIdentitySecurityLogAppService
{
    private readonly IIdentitySecurityLogAppService _service;

    public IdentitySecurityLogController(IIdentitySecurityLogAppService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public virtual Task<IdentitySecurityLogDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentitySecurityLogDto>> GetListAsync([FromQuery] IdentitySecurityLogPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("my")]
    public virtual Task<PagedResultDto<IdentitySecurityLogDto>> GetListByCurrentUserAsync([FromQuery] IdentitySecurityLogPagedListRequestDto input)
    {
        return _service.GetListByCurrentUserAsync(input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }
}
