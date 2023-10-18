using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/organization-units")]
public class OrganizationUnitController : IdentityBaseController, IOrganizationUnitAppService
{
    private readonly IOrganizationUnitAppService _service;

    public OrganizationUnitController(IOrganizationUnitAppService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }

    [HttpGet("children/{parentId}")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetChildrenListAsync(Guid parentId)
    {
        return _service.GetChildrenListAsync(parentId);
    }

    [HttpGet("children/{parentId}/all")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenListAsync(Guid parentId)
    {
        return _service.GetAllChildrenListAsync(parentId);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<OrganizationUnitDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}/users")]
    public virtual Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, OrganizationUnitUserPagedListRequestDto input)
    {
        return _service.GetUsersAsync(id, input);
    }

    [HttpGet("{id}/assignable-users")]
    public virtual Task<PagedResultDto<IdentityUserDto>> GetAssignableUsersAsync(Guid id, OrganizationUnitAssignableUserPagedListRequestDto input)
    {
        return _service.GetAssignableUsersAsync(id, input);
    }

    [HttpPost("{id}/users")]
    public virtual Task AddUsersAsync(Guid id, OrganizationUnitAddUserRequestDto input)
    {
        return _service.AddUsersAsync(id, input);
    }

    [HttpDelete("{id}/users/{userId}")]
    public virtual Task DeleteUserAsync(Guid id, Guid userId)
    {
        return _service.DeleteUserAsync(id, userId);
    }

    [HttpGet("{id}/assignable-roles")]
    public virtual Task<PagedResultDto<IdentityRoleDto>> GetAssignableRolesAsync(Guid id, OrganizationUnitAssignableRolePagedListRequestDto input)
    {
        return _service.GetAssignableRolesAsync(id, input);
    }

    [HttpGet("{id}/roles")]
    public virtual Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, OrganizationUnitRolePagedListRequestDto input)
    {
        return _service.GetRolesAsync(id, input);
    }

    [HttpPost("{id}/roles")]
    public virtual Task AddRolesAsync(Guid id, OrganizationUnitAddRoleRequestDto input)
    {
        return _service.AddRolesAsync(id, input);
    }

    [HttpDelete("{id}/roles/{roleId}")]
    public virtual Task DeleteRoleAsync(Guid id, Guid roleId)
    {
        return _service.DeleteRoleAsync(id, roleId);
    }
}
