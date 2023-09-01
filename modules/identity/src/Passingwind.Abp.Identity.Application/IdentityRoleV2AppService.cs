using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissions.Roles.Default)]
public class IdentityRoleV2AppService : IdentityRoleAppService, IIdentityRoleV2AppService
{
    protected IdentityUserManager UserManager { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }

    public IdentityRoleV2AppService(
        IdentityRoleManager roleManager,
        IIdentityRoleRepository roleRepository,
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityClaimTypeRepository identityClaimTypeRepository) : base(roleManager, roleRepository)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        IdentityClaimTypeRepository = identityClaimTypeRepository;
    }
     
    public virtual async Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await RoleRepository.GetAsync(id);

        return new ListResultDto<IdentityClaimDto>(entity.Claims.OrderBy(x => x.ClaimType).Select(x => new IdentityClaimDto()
        {
            ClaimType = x.ClaimType,
            ClaimValue = x.ClaimValue,
        }).ToList());
    }

    [Authorize(IdentityPermissions.Roles.Update)]
    public virtual async Task MoveAllUserAsync(Guid id, IdentityRoleMoveAllUserRequestDto input)
    {
        var entity = await RoleRepository.GetAsync(id);

        IdentityRole? targetRole = null;

        if (input.TargetId.HasValue)
        {
            targetRole = await RoleRepository.GetAsync(input.TargetId.Value);
        }

        var users = await UserRepository.GetListAsync(roleId: id);

        foreach (var user in users)
        {
            (await UserManager.RemoveFromRoleAsync(user, entity.Name)).CheckErrors();
            if (targetRole != null)
                (await UserManager.AddToRoleAsync(user, targetRole.Name)).CheckErrors();
        }
    }

    [Authorize(IdentityPermissionNamesV2.Roles.ManageClaims)]
    public virtual async Task UpdateClaimAsync(Guid id, IdentityRoleClaimAddOrUpdateDto input)
    {
        var entity = await RoleRepository.GetAsync(id);

        var sourceClaims = entity.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();

        if (input.Items?.Any() == true)
        {
            var targetCliams = input.Items.Select(x => new Claim(x.ClaimType, x.ClaimValue));

            foreach (var item in sourceClaims.Except(targetCliams, new ClaimEqualityComparer()))
            {
                entity.RemoveClaim(item);
            }

            foreach (var item in targetCliams)
            {
                if (entity.FindClaim(item) == null)
                {
                    entity.AddClaim(GuidGenerator, item);
                }
            }
        }
        else
        {
            entity.Claims.Clear();
        }

        await RoleRepository.UpdateAsync(entity);
    }

    public async Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync()
    {
        var list = await IdentityClaimTypeRepository.GetListAsync();

        return new ListResultDto<IdentityClaimTypeDto>(ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(list));
    }
}
