using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

[Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Default)]
public class DynamicPermissionDefinitionAppService : DynamicPermissionManagementAppService, IDynamicPermissionDefinitionAppService
{
    private readonly IDynamicPermissionDefinitionRepository _permissionDefinitionRepository;

    public DynamicPermissionDefinitionAppService(IDynamicPermissionDefinitionRepository permissionDefinitionRepository)
    {
        _permissionDefinitionRepository = permissionDefinitionRepository;
    }

    public virtual async Task<ListResultDto<DynamicPermissionDefinitionDto>> GetAllListAsync(DynamicPermissionDefinitionListRequestDto input)
    {
        var list = await _permissionDefinitionRepository.GetListAsync();

        return new ListResultDto<DynamicPermissionDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionDefinition>, List<DynamicPermissionDefinitionDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DynamicPermissionDefinitionDto>> GetListAsync(DynamicPermissionDefinitionPagedListRequestDto input)
    {
        var count = await _permissionDefinitionRepository.GetCountAsync();
        var list = await _permissionDefinitionRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount);

        return new PagedResultDto<DynamicPermissionDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionDefinition>, List<DynamicPermissionDefinitionDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DynamicPermissionDefinitionDto> GetAsync(Guid id)
    {
        var entity = await _permissionDefinitionRepository.GetAsync(id);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task<DynamicPermissionDefinitionDto> CreateAsync(DynamicPermissionDefinitionCreateDto input)
    {
        var entity = new DynamicPermissionDefinition(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.GroupId,
            input.ParentId)
        {
            Description = input.Description,
        };

        if (await _permissionDefinitionRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(DynamicPermissionManagementErrorCodes.PermissionNameExists).WithData("name", input.Name);
        }

        await _permissionDefinitionRepository.InsertAsync(entity);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task<DynamicPermissionDefinitionDto> UpdateAsync(Guid id, DynamicPermissionDefinitionUpdateDto input)
    {
        var entity = await _permissionDefinitionRepository.GetAsync(id);

        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;

        await _permissionDefinitionRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _permissionDefinitionRepository.DeleteAsync(id);
    }
}
