using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public interface IDynamicPermissionDefinitionAppService : IApplicationService
{
    Task<ListResultDto<DynamicPermissionDefinitionDto>> GetAllListAsync(DynamicPermissionDefinitionListRequestDto input);

    Task<PagedResultDto<DynamicPermissionDefinitionDto>> GetListAsync(DynamicPermissionDefinitionPagedListRequestDto input);

    Task<DynamicPermissionDefinitionDto> GetAsync(Guid id);

    Task<DynamicPermissionDefinitionDto> CreateAsync(DynamicPermissionDefinitionCreateDto input);

    Task<DynamicPermissionDefinitionDto> UpdateAsync(Guid id, DynamicPermissionDefinitionUpdateDto input);

    Task DeleteAsync(Guid id);
}
