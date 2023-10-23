using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClient;

[Area(IdentityClientRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityClientRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity-management/clients")]
public class IdentityClientController : IdentityClientControllerBase, IIdentityClientAppService
{
    private readonly IIdentityClientAppService _service;

    public IdentityClientController(IIdentityClientAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityClientDto>> GetListAsync(IdentityClientListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<IdentityClientDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<IdentityClientDto> CreateAsync(IdentityClientCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<IdentityClientDto> UpdateAsync(Guid id, IdentityClientUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpPost("{id}/validate")]
    public Task ValidateAsync(Guid id)
    {
        return _service.ValidateAsync(id);
    }
}
