using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public interface IIdentityRoleV2AppService : IIdentityRoleAppService
{
    Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync();

    Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id);

    Task UpdateClaimAsync(Guid id, IdentityRoleClaimAddOrUpdateDto input);

    Task MoveAllUserAsync(Guid id, IdentityRoleMoveAllUserRequestDto input);
}
