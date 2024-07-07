using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

[Authorize(PermissionManagementPermissions.DynamicPermissions.Default)]
public class DynamicPermissionDefinitionAppService : PermissionManagementAppService, IDynamicPermissionDefinitionAppService
{
    protected IDynamicPermissionDefinitionRepository PermissionDefinitionRepository { get; }
    protected DynamicPermissionManager DynamicPermissionManager { get; }

    public DynamicPermissionDefinitionAppService(IDynamicPermissionDefinitionRepository permissionDefinitionRepository, DynamicPermissionManager dynamicPermissionManager)
    {
        PermissionDefinitionRepository = permissionDefinitionRepository;
        DynamicPermissionManager = dynamicPermissionManager;
    }

    public virtual async Task<ListResultDto<DynamicPermissionDefinitionDto>> GetAllListAsync(DynamicPermissionDefinitionListRequestDto input)
    {
        var list = await PermissionDefinitionRepository.GetListAsync(groupId: input.GroupId, parentId: input.ParentId);

        return new ListResultDto<DynamicPermissionDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionDefinition>, List<DynamicPermissionDefinitionDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DynamicPermissionDefinitionDto>> GetListAsync(DynamicPermissionDefinitionPagedListRequestDto input)
    {
        var count = await PermissionDefinitionRepository.GetCountAsync(groupId: input.GroupId, parentId: input.ParentId);
        var list = await PermissionDefinitionRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, groupId: input.GroupId, parentId: input.ParentId);

        return new PagedResultDto<DynamicPermissionDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionDefinition>, List<DynamicPermissionDefinitionDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DynamicPermissionDefinitionDto> GetAsync(Guid id)
    {
        var entity = await PermissionDefinitionRepository.GetAsync(id);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task<DynamicPermissionDefinitionDto> CreateAsync(DynamicPermissionDefinitionCreateDto input)
    {
        var targetName = await DynamicPermissionManager.NormalizeNameAsync(input.Name);
        var entity = new DynamicPermissionDefinition(
            GuidGenerator.Create(),
            input.Name,
            targetName,
            input.DisplayName,
            input.IsEnabled,
            input.GroupId,
            input.ParentId)
        {
            Description = input.Description,
        };

        if (await PermissionDefinitionRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(PermissionManagementErrorCodes.PermissionNameExists).WithData("name", input.Name);
        }

        await PermissionDefinitionRepository.InsertAsync(entity);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task<DynamicPermissionDefinitionDto> UpdateAsync(Guid id, DynamicPermissionDefinitionUpdateDto input)
    {
        var entity = await PermissionDefinitionRepository.GetAsync(id);

        entity.IsEnabled = input.IsEnabled;
        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;

        await PermissionDefinitionRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DynamicPermissionDefinition, DynamicPermissionDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await PermissionDefinitionRepository.DeleteAsync(id);
    }
}
