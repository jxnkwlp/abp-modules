using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

[Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Default)]
public class DynamicPermissionGroupDefinitionAppService : DynamicPermissionManagementAppService, IDynamicPermissionGroupDefinitionAppService
{
    private readonly IDynamicPermissionGroupDefinitionRepository _permissionGroupDefinitionRepository;
    private readonly IDynamicPermissionDefinitionRepository _dynamicPermissionDefinitionRepository;

    public DynamicPermissionGroupDefinitionAppService(IDynamicPermissionGroupDefinitionRepository permissionGroupDefinitionRepository, IDynamicPermissionDefinitionRepository dynamicPermissionDefinitionRepository)
    {
        _permissionGroupDefinitionRepository = permissionGroupDefinitionRepository;
        _dynamicPermissionDefinitionRepository = dynamicPermissionDefinitionRepository;
    }

    public async Task<ListResultDto<DynamicPermissionGroupDefinitionDto>> GetAllListAsync()
    {
        var list = await _permissionGroupDefinitionRepository.GetListAsync();

        return new PagedResultDto<DynamicPermissionGroupDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionGroupDefinition>, List<DynamicPermissionGroupDefinitionDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DynamicPermissionGroupDefinitionDto>> GetListAsync(DynamicPermissionGroupDefinitionListRequestDto input)
    {
        var count = await _permissionGroupDefinitionRepository.GetCountAsync();
        var list = await _permissionGroupDefinitionRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting: nameof(DynamicPermissionGroupDefinition.Name));

        return new PagedResultDto<DynamicPermissionGroupDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionGroupDefinition>, List<DynamicPermissionGroupDefinitionDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DynamicPermissionGroupDefinitionDto> GetAsync(Guid id)
    {
        var entity = await _permissionGroupDefinitionRepository.GetAsync(id);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task<DynamicPermissionGroupDefinitionDto> CreateAsync(DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        var entity = new DynamicPermissionGroupDefinition(GuidGenerator.Create(), input.Name, input.DisplayName);

        if (await _permissionGroupDefinitionRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(DynamicPermissionManagementErrorCodes.PermissionGroupNameExists).WithData("name", input.Name);
        }

        await _permissionGroupDefinitionRepository.InsertAsync(entity);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task<DynamicPermissionGroupDefinitionDto> UpdateAsync(Guid id, DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        var entity = await _permissionGroupDefinitionRepository.GetAsync(id);

        entity.DisplayName = input.DisplayName;
        entity.SetName(input.Name);

        if (await _permissionGroupDefinitionRepository.IsNameExistsAsync(input.Name, new Guid[] { entity.Id }))
        {
            throw new BusinessException(DynamicPermissionManagementErrorCodes.PermissionGroupNameExists).WithData("name", input.Name);
        }

        await _permissionGroupDefinitionRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(DynamicPermissionManagementPermissions.PermissionDefinition.Manage)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var count = await _dynamicPermissionDefinitionRepository.GetCountAsync(groupId: id);
        if (count > 0)
        {
            throw new BusinessException(DynamicPermissionManagementErrorCodes.PermissionGroupHasPermissions);
        }

        await _permissionGroupDefinitionRepository.DeleteAsync(id);
    }
}
