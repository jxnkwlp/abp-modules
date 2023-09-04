using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/users")]
public class IdentityUserV2Controller : IdentityBaseController, IIdentityUserV2AppService
{
    private readonly IIdentityUserV2AppService _service;

    public IdentityUserV2Controller(IIdentityUserV2AppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityUserV2Dto>> GetListAsync(IdentityUserPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
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
    public virtual Task UpdateClaimAsync(Guid id, IdentityUserClaimAddOrUpdateDto input)
    {
        return _service.UpdateClaimAsync(id, input);
    }

    [HttpPost("{id}/lock")]
    public virtual Task LockAsync(Guid id, IdentityUserSetLockoutRequestDto input)
    {
        return _service.LockAsync(id, input);
    }

    [HttpPost("{id}/unlock")]
    public virtual Task UnLockAsync(Guid id)
    {
        return _service.UnLockAsync(id);
    }

    [HttpPut("{id}/change-password")]
    public virtual Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordDto input)
    {
        return _service.UpdatePasswordAsync(id, input);
    }

    [HttpGet("{id}/two-factor-enabled")]
    public virtual Task<IdentityUserTwoFactorEnabledDto> GetTwoFactorEnabledAsync(Guid id)
    {
        return _service.GetTwoFactorEnabledAsync(id);
    }

    [HttpPut("{id}/two-factor-enabled")]
    public virtual Task UpdateTwoFactorEnabledAsync(Guid id, IdentityUserTwoFactorEnabledDto input)
    {
        return _service.UpdateTwoFactorEnabledAsync(id, input);
    }

    [HttpGet("assignable-organization-units")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetAssignableOrganizationUnitsAsync()
    {
        return _service.GetAssignableOrganizationUnitsAsync();
    }

    [HttpGet("{id}/organization-units")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        return _service.GetOrganizationUnitsAsync(id);
    }

    [HttpPut("{id}/organization-units")]
    public virtual Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserUpdateOrganizationUnitsDto input)
    {
        return _service.UpdateOrganizationUnitsAsync(id, input);
    }

    [HttpGet("{id}/roles")]
    public Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        return _service.GetRolesAsync(id);
    }

    [HttpGet("{id}/assignable-roles")]
    public Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        return _service.GetAssignableRolesAsync();
    }

    [HttpPut("{id}/roles")]
    public Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        return _service.UpdateRolesAsync(id, input);
    }

    [HttpGet("by-username/{userName}")]
    public Task<IdentityUserV2Dto?> FindByUsernameAsync(string userName)
    {
        return _service.FindByUsernameAsync(userName);
    }

    [HttpGet("by-email/{userName}")]
    public Task<IdentityUserV2Dto?> FindByEmailAsync(string email)
    {
        return _service.FindByEmailAsync(email);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpPost]
    public Task<IdentityUserV2Dto> CreateAsync(IdentityUserCreateV2Dto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public Task<IdentityUserV2Dto> UpdateAsync(Guid id, IdentityUserUpdateV2Dto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpGet("{id}")]
    public Task<IdentityUserV2Dto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPut("{id}/clear-password")]
    public Task ClearPasswordAsync(Guid id)
    {
        return _service.ClearPasswordAsync(id);
    }
}
