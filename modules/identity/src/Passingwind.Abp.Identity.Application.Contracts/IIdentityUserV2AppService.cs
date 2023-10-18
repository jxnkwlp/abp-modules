using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public interface IIdentityUserV2AppService : ICrudAppService<IdentityUserV2Dto, Guid, IdentityUserPagedListRequestDto, IdentityUserCreateV2Dto, IdentityUserUpdateV2Dto>
{
    Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

    Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

    Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

    Task<IdentityUserV2Dto?> FindByUsernameAsync(string userName);

    Task<IdentityUserV2Dto?> FindByEmailAsync(string email);

    Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync();

    Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id);

    Task UpdateClaimAsync(Guid id, IdentityUserClaimAddOrUpdateDto input);

    Task LockAsync(Guid id, IdentityUserSetLockoutRequestDto input);

    Task UnLockAsync(Guid id);

    Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordDto input);

    Task ClearPasswordAsync(Guid id);

    Task UpdateEmailConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input);

    Task UpdatePhoneNumberConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input);

    Task<IdentityUserTwoFactorEnabledDto> GetTwoFactorEnabledAsync(Guid id);

    Task UpdateTwoFactorEnabledAsync(Guid id, IdentityUserTwoFactorEnabledDto input);

    Task<ListResultDto<OrganizationUnitDto>> GetAssignableOrganizationUnitsAsync();

    Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);

    Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserUpdateOrganizationUnitsDto input);

    Task ResetAuthenticatorAsync(Guid id);

    Task<IdentityUserShouldChangePasswordDto> GetShouldChangePasswordAsync(Guid id);
}
