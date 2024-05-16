using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
namespace Passingwind.Abp.Identity;

[ControllerName("IdentityRole")]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/roles")]
public class IdentityRoleV2Controller : IdentityBaseController, IIdentityRoleV2AppService
{
    private readonly IIdentityRoleV2AppService _service;

    public IdentityRoleV2Controller(IIdentityRoleV2AppService service)
    {
        _service = service;
    }

    [HttpGet("assignable-claims")]
    public virtual Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync()
    {
        return _service.GetAssignableClaimsAsync();
    }

    [HttpGet("{id}/claims")]
    public virtual Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        return _service.GetClaimsAsync(id);
    }

    [HttpPut("{id}/claims")]
    public virtual Task UpdateClaimAsync(Guid id, IdentityRoleClaimAddOrUpdateDto input)
    {
        return _service.UpdateClaimAsync(id, input);
    }

    [HttpPost("{id}/move-all-users")]
    public virtual Task MoveAllUserAsync(Guid id, IdentityRoleMoveAllUserRequestDto input)
    {
        return _service.MoveAllUserAsync(id, input);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }

    [HttpGet("{id}")]
    public virtual Task<IdentityRoleDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }
}
