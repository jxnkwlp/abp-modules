using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public interface IOrganizationUnitAppService : IApplicationService
{
    Task<ListResultDto<OrganizationUnitDto>> GetChildrenListAsync(Guid parentId);
    Task<ListResultDto<OrganizationUnitDto>> GetAllChildrenListAsync(Guid parentId);

    Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync();
    Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitPagedListRequestDto input);
    Task<OrganizationUnitDto> GetAsync(Guid id);
    Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input);
    Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input);
    Task DeleteAsync(Guid id);

    Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, OrganizationUnitUserPagedListRequestDto input);
    Task<PagedResultDto<IdentityUserDto>> GetAssignableUsersAsync(Guid id, OrganizationUnitAssignableUserPagedListRequestDto input);
    Task AddUsersAsync(Guid id, OrganizationUnitAddUserRequestDto input);
    Task DeleteUserAsync(Guid id, Guid userId);

    Task<PagedResultDto<IdentityRoleDto>> GetAssignableRolesAsync(Guid id, OrganizationUnitAssignableRolePagedListRequestDto input);
    Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, OrganizationUnitRolePagedListRequestDto input);
    Task AddRolesAsync(Guid id, OrganizationUnitAddRoleRequestDto input);
    Task DeleteRoleAsync(Guid id, Guid roleId);
}
