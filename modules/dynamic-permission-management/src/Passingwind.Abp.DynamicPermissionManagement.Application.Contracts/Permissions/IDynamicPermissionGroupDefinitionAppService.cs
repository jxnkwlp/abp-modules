using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public interface IDynamicPermissionGroupDefinitionAppService : IApplicationService
{
    Task<ListResultDto<DynamicPermissionGroupDefinitionDto>> GetAllListAsync();

    Task<PagedResultDto<DynamicPermissionGroupDefinitionDto>> GetListAsync(DynamicPermissionGroupDefinitionListRequestDto input);

    Task<DynamicPermissionGroupDefinitionDto> GetAsync(Guid id);

    Task<DynamicPermissionGroupDefinitionDto> CreateAsync(DynamicPermissionGroupDefinitionCreateOrUpdateDto input);

    Task<DynamicPermissionGroupDefinitionDto> UpdateAsync(Guid id, DynamicPermissionGroupDefinitionCreateOrUpdateDto input);

    Task DeleteAsync(Guid id);
}
