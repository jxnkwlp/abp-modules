using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

[Authorize(PermissionManagementPermissions.DynamicPermissions.Default)]
public class DynamicPermissionGroupDefinitionAppService : PermissionManagementAppService, IDynamicPermissionGroupDefinitionAppService
{
    protected IDynamicPermissionGroupDefinitionRepository PermissionGroupDefinitionRepository { get; }
    protected IDynamicPermissionDefinitionRepository DynamicPermissionDefinitionRepository { get; }
    protected DynamicPermissionManager DynamicPermissionManager { get; }

    public DynamicPermissionGroupDefinitionAppService(IDynamicPermissionGroupDefinitionRepository permissionGroupDefinitionRepository, IDynamicPermissionDefinitionRepository dynamicPermissionDefinitionRepository, DynamicPermissionManager dynamicPermissionManager)
    {
        PermissionGroupDefinitionRepository = permissionGroupDefinitionRepository;
        DynamicPermissionDefinitionRepository = dynamicPermissionDefinitionRepository;
        DynamicPermissionManager = dynamicPermissionManager;
    }

    public virtual async Task<ListResultDto<DynamicPermissionGroupDefinitionDto>> GetAllListAsync()
    {
        var list = await PermissionGroupDefinitionRepository.GetListAsync();

        return new PagedResultDto<DynamicPermissionGroupDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionGroupDefinition>, List<DynamicPermissionGroupDefinitionDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DynamicPermissionGroupDefinitionDto>> GetListAsync(DynamicPermissionGroupDefinitionListRequestDto input)
    {
        var count = await PermissionGroupDefinitionRepository.GetCountAsync();
        var list = await PermissionGroupDefinitionRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, sorting: nameof(DynamicPermissionGroupDefinition.Name));

        return new PagedResultDto<DynamicPermissionGroupDefinitionDto>()
        {
            Items = ObjectMapper.Map<List<DynamicPermissionGroupDefinition>, List<DynamicPermissionGroupDefinitionDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DynamicPermissionGroupDefinitionDto> GetAsync(Guid id)
    {
        var entity = await PermissionGroupDefinitionRepository.GetAsync(id);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task<DynamicPermissionGroupDefinitionDto> CreateAsync(DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        var targetName = await DynamicPermissionManager.NormalizeNameAsync(input.Name);
        var entity = new DynamicPermissionGroupDefinition(GuidGenerator.Create(), input.Name, targetName, input.DisplayName);

        if (await PermissionGroupDefinitionRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(PermissionManagementErrorCodes.PermissionGroupNameExists).WithData("name", input.Name);
        }

        await PermissionGroupDefinitionRepository.InsertAsync(entity);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task<DynamicPermissionGroupDefinitionDto> UpdateAsync(Guid id, DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        var entity = await PermissionGroupDefinitionRepository.GetAsync(id);

        if (await PermissionGroupDefinitionRepository.IsNameExistsAsync(input.Name, new Guid[] { entity.Id }))
        {
            throw new BusinessException(PermissionManagementErrorCodes.PermissionGroupNameExists).WithData("name", input.Name);
        }

        var targetName = await DynamicPermissionManager.NormalizeNameAsync(input.Name);

        entity.DisplayName = input.DisplayName;
        entity.SetName(input.Name, targetName);

        await PermissionGroupDefinitionRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DynamicPermissionGroupDefinition, DynamicPermissionGroupDefinitionDto>(entity);
    }

    [Authorize(PermissionManagementPermissions.DynamicPermissions.Manage)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var count = await DynamicPermissionDefinitionRepository.GetCountAsync(groupId: id);
        if (count > 0)
        {
            throw new BusinessException(PermissionManagementErrorCodes.PermissionGroupHasPermissions);
        }

        await PermissionGroupDefinitionRepository.DeleteAsync(id);
    }
}
