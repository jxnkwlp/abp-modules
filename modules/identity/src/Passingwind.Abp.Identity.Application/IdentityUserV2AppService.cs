using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using IdentityUserManagerV2 = Passingwind.Abp.Identity.IdentityUserManager;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissions.Users.Default)]
public class IdentityUserV2AppService : IdentityUserAppService, IIdentityUserV2AppService
{
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected IdentityUserManagerV2 UserManagerV2 { get; }

    public IdentityUserV2AppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        IIdentityClaimTypeRepository identityClaimTypeRepository,
        IOrganizationUnitRepository organizationUnitRepository,
        IdentityUserManagerV2 userManagerV2, IPermissionChecker permissionChecker) : base(userManager, userRepository, roleRepository, identityOptions, permissionChecker)
    {
        IdentityClaimTypeRepository = identityClaimTypeRepository;
        OrganizationUnitRepository = organizationUnitRepository;
        UserManagerV2 = userManagerV2;
    }

    protected override async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        await base.UpdateUserByInput(user, input);

        if (input is IdentityUserCreateOrUpdateV2Dto inputV2)
        {
            if (inputV2.ShouldChangePasswordOnNextLogin.HasValue)
                user.SetShouldChangePasswordOnNextLogin(inputV2.ShouldChangePasswordOnNextLogin.Value);

            inputV2.OrganizationUnitIds ??= Array.Empty<Guid>();

            await UserManager.SetOrganizationUnitsAsync(user, inputV2.OrganizationUnitIds);

            if (user.EmailConfirmed != inputV2.EmailConfirmed)
            {
                user.SetEmailConfirmed(inputV2.EmailConfirmed);
            }

            if (user.PhoneNumberConfirmed != inputV2.PhoneNumberConfirmed)
            {
                user.SetEmailConfirmed(inputV2.PhoneNumberConfirmed);
            }
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                user.SetPhoneNumberConfirmed(false);
            }
        }
    }

    public virtual async Task<ListResultDto<IdentityClaimTypeDto>> GetAssignableClaimsAsync()
    {
        var list = await IdentityClaimTypeRepository.GetListAsync();

        return new ListResultDto<IdentityClaimTypeDto>(ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(list));
    }

    public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAssignableOrganizationUnitsAsync()
    {
        var list = await OrganizationUnitRepository.GetListAsync(nameof(OrganizationUnit.Code));

        return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }

    public virtual async Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
    {
        var entity = await UserRepository.GetAsync(id);

        return new ListResultDto<IdentityClaimDto>(entity.Claims.OrderBy(x => x.ClaimType).Select(x => new IdentityClaimDto()
        {
            ClaimType = x.ClaimType,
            ClaimValue = x.ClaimValue,
        }).ToList());
    }

    public virtual async Task<PagedResultDto<IdentityUserV2Dto>> GetListAsync(IdentityUserPagedListRequestDto input)
    {
        var count = await UserRepository.GetCountAsync(
            filter: input.Filter,
            roleId: input.RoleId,
            organizationUnitId: input.OrganizationUnitId,
            emailAddress: input.EmailAddress,
            isLockedOut: input.IsLockedOut,
            notActive: !input.IsActive,
            isExternal: input.IsExternal,
            maxCreationTime: input.MaxCreationTime,
            minCreationTime: input.MinCreationTime);
        var list = await UserRepository.GetListAsync(
            input.Sorting ?? nameof(IdentityUser.CreationTime) + " desc",
            input.MaxResultCount,
            input.SkipCount,
            filter: input.Filter,
            roleId: input.RoleId,
            organizationUnitId: input.OrganizationUnitId,
            emailAddress: input.EmailAddress,
            isLockedOut: input.IsLockedOut,
            notActive: !input.IsActive,
            isExternal: input.IsExternal,
            maxCreationTime: input.MaxCreationTime,
            minCreationTime: input.MinCreationTime);

        return new PagedResultDto<IdentityUserV2Dto>(
            count,
            ObjectMapper.Map<List<IdentityUser>, List<IdentityUserV2Dto>>(list)
        );
    }

    public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
    {
        var entity = await UserRepository.GetAsync(id);

        var list = await UserRepository.GetOrganizationUnitsAsync(id);

        return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }

    public virtual async Task<IdentityUserTwoFactorEnabledDto> GetTwoFactorEnabledAsync(Guid id)
    {
        var entity = await UserRepository.GetAsync(id);

        return new IdentityUserTwoFactorEnabledDto()
        {
            Enabled = entity.TwoFactorEnabled,
        };
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task LockAsync(Guid id, IdentityUserSetLockoutRequestDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        (await UserManager.SetLockoutEnabledAsync(entity, true)).CheckErrors();
        (await UserManager.SetLockoutEndDateAsync(entity, input.EndTime)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task SetClaimAsync(Guid id, IdentityUserClaimAddOrUpdateDto input)
    {
        var entity = await UserRepository.GetAsync(id);

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

        await UserRepository.UpdateAsync(entity);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UnLockAsync(Guid id)
    {
        var entity = await UserRepository.GetAsync(id);

        if (await UserManager.IsLockedOutAsync(entity))
        {
            (await UserManager.SetLockoutEndDateAsync(entity, null)).CheckErrors();
        }
    }

    [Authorize(IdentityPermissionNamesV2.Users.ManageClaims)]
    public virtual async Task UpdateClaimAsync(Guid id, IdentityUserClaimAddOrUpdateDto input)
    {
        var entity = await UserRepository.GetAsync(id);

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

        await UserRepository.UpdateAsync(entity);
    }

    [Authorize(IdentityPermissionNamesV2.Users.ManageOrganizations)]
    public virtual async Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserUpdateOrganizationUnitsDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        await UserManager.SetOrganizationUnitsAsync(entity, input.Ids ?? Array.Empty<Guid>());
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdatePasswordAsync(Guid id, IdentityUserUpdatePasswordDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        if (await UserManager.HasPasswordAsync(entity))
        {
            (await UserManager.RemovePasswordAsync(entity)).CheckErrors();
        }

        (await UserManager.AddPasswordAsync(entity, input.Password)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task ClearPasswordAsync(Guid id)
    {
        var entity = await UserRepository.GetAsync(id);

        if (await UserManager.HasPasswordAsync(entity))
        {
            (await UserManager.RemovePasswordAsync(entity)).CheckErrors();
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateTwoFactorEnabledAsync(Guid id, IdentityUserTwoFactorEnabledDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        (await UserManager.SetTwoFactorEnabledAsync(entity, input.Enabled)).CheckErrors();
    }

    async Task<IdentityUserV2Dto?> IIdentityUserV2AppService.FindByUsernameAsync(string userName)
    {
        var entity = await UserManager.FindByNameAsync(userName);
        if (entity == null)
            throw new EntityNotFoundException(typeof(IdentityUser));

        return ObjectMapper.Map<IdentityUser, IdentityUserV2Dto>(entity);
    }

    async Task<IdentityUserV2Dto?> IIdentityUserV2AppService.FindByEmailAsync(string email)
    {
        var entity = await UserManager.FindByEmailAsync(email);
        if (entity == null)
            throw new EntityNotFoundException(typeof(IdentityUser));

        return ObjectMapper.Map<IdentityUser, IdentityUserV2Dto>(entity);
    }

    async Task<IdentityUserV2Dto> IReadOnlyAppService<IdentityUserV2Dto, IdentityUserV2Dto, Guid, IdentityUserPagedListRequestDto>.GetAsync(Guid id)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserV2Dto>(
            await UserManager.GetByIdAsync(id)
        );
    }

    [Authorize(IdentityPermissions.Users.Create)]
    public virtual async Task<IdentityUserV2Dto> CreateAsync(IdentityUserCreateV2Dto input)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await UserManager.AddPasswordAsync(user, input.Password!)).CheckErrors();
        }

        await UpdateUserByInput(user, input);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserV2Dto>(user);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task<IdentityUserV2Dto> UpdateAsync(Guid id, IdentityUserUpdateV2Dto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

        await UpdateUserByInput(user, input);
        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await UserManager.RemovePasswordAsync(user)).CheckErrors();
            (await UserManager.AddPasswordAsync(user, input.Password!)).CheckErrors();
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserV2Dto>(user);
    }

    [Authorize(IdentityPermissionNamesV2.Users.ManageRoles)]
    public override async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        await base.UpdateRolesAsync(id, input);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task ResetAuthenticatorAsync(Guid id)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        await UserManagerV2.RemoveAuthenticatorAsync(user);
    }

    public virtual async Task<IdentityUserShouldChangePasswordDto> GetShouldChangePasswordAsync(Guid id)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        return new IdentityUserShouldChangePasswordDto
        {
            Result = await UserManager.ShouldPeriodicallyChangePasswordAsync(user),
        };
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateEmailConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        entity.SetEmailConfirmed(input.Confirmed);

        await UserManager.UpdateAsync(entity);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdatePhoneNumberConfirmedAsync(Guid id, IdentityUserUpdateConfirmedDto input)
    {
        var entity = await UserRepository.GetAsync(id);

        entity.SetPhoneNumberConfirmed(input.Confirmed);

        await UserManager.UpdateAsync(entity);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdateRolesAsync(IdentityUserBatchUpdateRolesDto input)
    {
        foreach (var id in input.UserIds)
        {
            var user = await UserManager.GetByIdAsync(id);

            if (input.RoleNames?.Any() == true)
            {
                if (input.Override)
                {
                    (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
                }
                else
                {
                    List<string> roleNames = (await UserManager.GetRolesAsync(user)).ToList();

                    roleNames.AddRange(input.RoleNames);

                    (await UserManager.SetRolesAsync(user, roleNames.Distinct())).CheckErrors();
                }
            }
            else
            {
                (await UserManager.SetRolesAsync(user, new string[0])).CheckErrors();
            }
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdateOrganizationUnitsAsync(IdentityUserBatchUpdateOrganizationUnitsDto input)
    {
        foreach (var id in input.UserIds)
        {
            var user = await UserManager.GetByIdAsync(id);

            if (input.OrganizationUnitIds?.Any() == true)
            {
                if (input.Override)
                {
                    await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnitIds);
                }
                else
                {
                    var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);

                    var ids = organizationUnits.ConvertAll(x => x.Id);

                    ids.AddRange(input.OrganizationUnitIds);

                    await UserManager.SetOrganizationUnitsAsync(user, ids.ToArray());
                }
            }
            else
            {
                await UserManager.SetOrganizationUnitsAsync(user, new Guid[0]);
            }
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchClearPasswordAsync(IdentityUserBatchClearPasswordDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            if (await UserManager.HasPasswordAsync(entity))
            {
                (await UserManager.RemovePasswordAsync(entity)).CheckErrors();
            }
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdateTwoFactorEnabledAsync(IdentityUserBatchUpdateTwoFactorEnabledDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            (await UserManager.SetTwoFactorEnabledAsync(entity, input.Enabled)).CheckErrors();
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchLockAsync(IdentityUserBatchLockDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            (await UserManager.SetLockoutEnabledAsync(entity, true)).CheckErrors();
            (await UserManager.SetLockoutEndDateAsync(entity, input.EndTime)).CheckErrors();
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUnlockAsync(IdentityUserBatchUnlockDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            if (await UserManager.IsLockedOutAsync(entity))
            {
                (await UserManager.SetLockoutEndDateAsync(entity, null)).CheckErrors();
            }
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdateEmailConfirmedAsync(IdentityUserBatchUpdateConfirmedDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            entity.SetEmailConfirmed(input.Confirmed);

            await UserManager.UpdateAsync(entity);
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdatePhoneNumberConfirmedAsync(IdentityUserBatchUpdateConfirmedDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            entity.SetPhoneNumberConfirmed(input.Confirmed);

            await UserManager.UpdateAsync(entity);
        }
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task BatchUpdateChangePasswordOnNextLoginAsync(IdentityUserBatchInputDto input)
    {
        foreach (var id in input.UserIds)
        {
            var entity = await UserRepository.GetAsync(id);

            entity.SetShouldChangePasswordOnNextLogin(true);

            await UserManager.UpdateAsync(entity);
        }
    }
}
