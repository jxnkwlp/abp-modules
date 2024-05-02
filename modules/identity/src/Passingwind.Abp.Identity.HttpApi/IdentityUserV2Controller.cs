using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Asp.Versioning;
namespace Passingwind.Abp.Identity;

[ControllerName("IdentityUser")]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Route("api/identity/users")]
public class IdentityUserV2Controller : IdentityBaseController, IIdentityUserV2AppService
{
    protected IIdentityUserV2AppService Service { get; }

    public IdentityUserV2Controller(IIdentityUserV2AppService service)
    {
        Service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<IdentityUserV2Dto>> GetListAsync(IdentityUserPagedListRequestDto input)
    {
        return Service.GetListAsync(input);
    }

    [HttpGet("assignable-claims")]
    public virtual Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync()
    {
        return Service.GetAssignableClaimsAsync();
    }

    [HttpGet("{id}/claims")]
    public virtual Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        return Service.GetClaimsAsync(id);
    }

    [HttpPut("{id}/claims")]
    public virtual Task UpdateClaimAsync(Guid id, IdentityUserClaimAddOrUpdateDto input)
    {
        return Service.UpdateClaimAsync(id, input);
    }

    [HttpPost("{id}/lock")]
    public virtual Task LockAsync(Guid id, IdentityUserSetLockoutRequestDto input)
    {
        return Service.LockAsync(id, input);
    }

    [HttpPost("{id}/unlock")]
    public virtual Task UnLockAsync(Guid id)
    {
        return Service.UnLockAsync(id);
    }

    [HttpPut("{id}/change-password")]
    public virtual Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordDto input)
    {
        return Service.UpdatePasswordAsync(id, input);
    }

    [HttpGet("{id}/two-factor-enabled")]
    public virtual Task<IdentityUserTwoFactorEnabledDto> GetTwoFactorEnabledAsync(Guid id)
    {
        return Service.GetTwoFactorEnabledAsync(id);
    }

    [HttpPut("{id}/two-factor-enabled")]
    public virtual Task UpdateTwoFactorEnabledAsync(Guid id, IdentityUserTwoFactorEnabledDto input)
    {
        return Service.UpdateTwoFactorEnabledAsync(id, input);
    }

    [HttpGet("assignable-organization-units")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetAssignableOrganizationUnitsAsync()
    {
        return Service.GetAssignableOrganizationUnitsAsync();
    }

    [HttpGet("{id}/organization-units")]
    public virtual Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        return Service.GetOrganizationUnitsAsync(id);
    }

    [HttpPut("{id}/organization-units")]
    public virtual Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserUpdateOrganizationUnitsDto input)
    {
        return Service.UpdateOrganizationUnitsAsync(id, input);
    }

    [HttpGet("{id}/roles")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        return Service.GetRolesAsync(id);
    }

    [HttpGet("assignable-roles")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        return Service.GetAssignableRolesAsync();
    }

    [HttpPut("{id}/roles")]
    public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        return Service.UpdateRolesAsync(id, input);
    }

    [HttpGet("by-username/{userName}")]
    public virtual Task<IdentityUserV2Dto?> FindByUsernameAsync(string userName)
    {
        return Service.FindByUsernameAsync(userName);
    }

    [HttpGet("by-email/{userName}")]
    public virtual Task<IdentityUserV2Dto?> FindByEmailAsync(string email)
    {
        return Service.FindByEmailAsync(email);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpPost]
    public virtual Task<IdentityUserV2Dto> CreateAsync(IdentityUserCreateV2Dto input)
    {
        return Service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<IdentityUserV2Dto> UpdateAsync(Guid id, IdentityUserUpdateV2Dto input)
    {
        return Service.UpdateAsync(id, input);
    }

    [HttpGet("{id}")]
    public virtual Task<IdentityUserV2Dto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpPut("{id}/clear-password")]
    public virtual Task ClearPasswordAsync(Guid id)
    {
        return Service.ClearPasswordAsync(id);
    }

    [HttpPost("{id}/reset-authenticator")]
    public virtual Task ResetAuthenticatorAsync(Guid id)
    {
        return Service.ResetAuthenticatorAsync(id);
    }

    [HttpGet("{id}/should-change-password")]
    public virtual Task<IdentityUserShouldChangePasswordDto> GetShouldChangePasswordAsync(Guid id)
    {
        return Service.GetShouldChangePasswordAsync(id);
    }

    [HttpPut("{id}/email-confirmed")]
    public virtual Task UpdateEmailConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input)
    {
        return Service.UpdateEmailConfirmedAsync(id, input);
    }

    [HttpPut("{id}/phonenumber-confirmed")]
    public virtual Task UpdatePhoneNumberConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input)
    {
        return Service.UpdatePhoneNumberConfirmedAsync(id, input);
    }

    [HttpPut("batch/roles")]
    public virtual Task BatchUpdateRolesAsync(IdentityUserBatchUpdateRolesDto input)
    {
        return Service.BatchUpdateRolesAsync(input);
    }

    [HttpPut("batch/organization-units")]
    public virtual Task BatchUpdateOrganizationUnitsAsync(IdentityUserBatchUpdateOrganizationUnitsDto input)
    {
        return Service.BatchUpdateOrganizationUnitsAsync(input);
    }

    [HttpPut("batch/clear-password")]
    public virtual Task BatchClearPasswordAsync(IdentityUserBatchClearPasswordDto input)
    {
        return Service.BatchClearPasswordAsync(input);
    }

    [HttpPut("batch/two-factor-enabled")]
    public virtual Task BatchUpdateTwoFactorEnabledAsync(IdentityUserBatchUpdateTwoFactorEnabledDto input)
    {
        return Service.BatchUpdateTwoFactorEnabledAsync(input);
    }

    [HttpPut("batch/lock")]
    public virtual Task BatchLockAsync(IdentityUserBatchLockDto input)
    {
        return Service.BatchLockAsync(input);
    }

    [HttpPut("batch/unlock")]
    public virtual Task BatchUnlockAsync(IdentityUserBatchUnlockDto input)
    {
        return Service.BatchUnlockAsync(input);
    }

    [HttpPut("batch/email-confirmed")]
    public virtual Task BatchUpdateEmailConfirmedAsync(IdentityUserBatchUpdateConfirmedDto input)
    {
        return Service.BatchUpdateEmailConfirmedAsync(input);
    }

    [HttpPut("batch/phonenumber-confirmed")]
    public virtual Task BatchUpdatePhoneNumberConfirmedAsync(IdentityUserBatchUpdateConfirmedDto input)
    {
        return Service.BatchUpdatePhoneNumberConfirmedAsync(input);
    }

    [HttpPut("batch/change-password-on-next-login")]
    public virtual Task BatchUpdateChangePasswordOnNextLoginAsync(IdentityUserBatchInputDto input)
    {
        return Service.BatchUpdateChangePasswordOnNextLoginAsync(input);
    }
}
