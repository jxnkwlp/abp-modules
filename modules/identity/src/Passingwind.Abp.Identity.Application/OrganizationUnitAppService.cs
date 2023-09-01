using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissionNamesV2.OrganizationUnits.Default)]
public class OrganizationUnitAppService : IdentityAppBaseService, IOrganizationUnitAppService
{
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected OrganizationUnitManager OrganizationUnitManager { get; }
    protected IdentityUserManager UserManager { get; set; }

    public OrganizationUnitAppService(
        IOrganizationUnitRepository organizationUnitRepository,
        OrganizationUnitManager organizationUnitManager,
        IdentityUserManager userManager)
    {
        OrganizationUnitRepository = organizationUnitRepository;
        OrganizationUnitManager = organizationUnitManager;
        UserManager = userManager;
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.ManageUsers)]
    public virtual async Task AddUsersAsync(Guid id, OrganizationUnitAddUserRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        foreach (var userId in input.UserIds ?? Array.Empty<Guid>())
        {
            await UserManager.AddToOrganizationUnitAsync(userId, entity.Id);
        }
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.ManageRoles)]
    public virtual async Task AddRolesAsync(Guid id, OrganizationUnitAddRoleRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        foreach (var roleId in input.RoleIds ?? Array.Empty<Guid>())
        {
            await OrganizationUnitManager.AddRoleToOrganizationUnitAsync(roleId, entity.Id);
        }
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.Manage)]
    public virtual async Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
    {
        var entity = new OrganizationUnit(GuidGenerator.Create(), input.DisplayName, input.ParentId, CurrentTenant.Id);

        await OrganizationUnitManager.CreateAsync(entity);

        return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(entity);
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await OrganizationUnitManager.DeleteAsync(id);
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.ManageUsers)]
    public virtual async Task DeleteUserAsync(Guid id, Guid userId)
    {
        await UserManager.RemoveFromOrganizationUnitAsync(userId, id);
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.ManageRoles)]
    public virtual async Task DeleteRoleAsync(Guid id, Guid roleId)
    {
        await OrganizationUnitManager.RemoveRoleFromOrganizationUnitAsync(roleId, id);
    }

    public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync()
    {
        var list = await OrganizationUnitRepository.GetListAsync(false, default);
        return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }

    public virtual async Task<PagedResultDto<IdentityUserDto>> GetAssignableUsersAsync(Guid id, OrganizationUnitAssignableUserPagedListRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        var count = await OrganizationUnitRepository.GetUnaddedUsersCountAsync(entity, filter: input.Filter);
        var list = await OrganizationUnitRepository.GetUnaddedUsersAsync(
            entity,
            sorting: input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            filter: input.Filter);

        return new PagedResultDto<IdentityUserDto>(count, ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list));
    }

    public virtual async Task<PagedResultDto<IdentityRoleDto>> GetAssignableRolesAsync(Guid id, OrganizationUnitAssignableRolePagedListRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        var count = await OrganizationUnitRepository.GetUnaddedRolesCountAsync(entity, filter: input.Filter);
        var list = await OrganizationUnitRepository.GetUnaddedRolesAsync(
            entity,
            sorting: input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            filter: input.Filter);

        return new PagedResultDto<IdentityRoleDto>(count, ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    public virtual async Task<OrganizationUnitDto> GetAsync(Guid id)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(entity);
    }

    public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitPagedListRequestDto input)
    {
        var count = await OrganizationUnitRepository.GetCountAsync();
        var list = await OrganizationUnitRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting ?? nameof(OrganizationUnit.Code));

        return new PagedResultDto<OrganizationUnitDto>(count, ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }

    public virtual async Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, OrganizationUnitUserPagedListRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        var count = await OrganizationUnitRepository.GetMembersCountAsync(entity, filter: input.Filter);
        var list = await OrganizationUnitRepository.GetMembersAsync(entity, input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

        return new PagedResultDto<IdentityUserDto>(count, ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list));
    }

    public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, OrganizationUnitRolePagedListRequestDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        var count = await OrganizationUnitRepository.GetRolesCountAsync(entity);
        var list = await OrganizationUnitRepository.GetRolesAsync(entity, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<IdentityRoleDto>(count, ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    [Authorize(IdentityPermissionNamesV2.OrganizationUnits.Manage)]
    public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
    {
        var entity = await OrganizationUnitRepository.GetAsync(id);

        entity.DisplayName = input.DisplayName;

        await OrganizationUnitManager.UpdateAsync(entity);

        return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(entity);
    }

    public virtual async Task<ListResultDto<OrganizationUnitDto>> GetChildrenListAsync(Guid parentId)
    {
        var list = await OrganizationUnitRepository.GetChildrenAsync(parentId);

        return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }

    public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenListAsync(Guid parentId)
    {
        var parent = await OrganizationUnitRepository.GetAsync(parentId);

        var list = await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(parent.Code, parent.Id);

        return new ListResultDto<OrganizationUnitDto>(ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list));
    }
}
