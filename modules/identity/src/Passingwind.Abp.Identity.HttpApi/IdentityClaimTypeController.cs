using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/claim-types")]
public class IdentityClaimTypeController : IdentityBaseController, IIdentityClaimTypeAppService
{
    private readonly IIdentityClaimTypeAppService _service;

    public IdentityClaimTypeController(IIdentityClaimTypeAppService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(IdentityClaimTypePagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }
}
