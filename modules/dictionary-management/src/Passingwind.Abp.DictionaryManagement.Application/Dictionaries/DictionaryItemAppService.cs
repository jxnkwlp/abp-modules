using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.DictionaryManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

[Authorize(DictionaryManagementPermissions.DictionaryItem.Default)]
public class DictionaryItemAppService : DictionaryManagementAppService, IDictionaryItemAppService
{
    protected IDictionaryItemRepository DictionaryItemRepository { get; }
    protected IDictionaryGroupRepository DictionaryGroupRepository { get; }

    public DictionaryItemAppService(IDictionaryItemRepository dictionaryItemRepository, IDictionaryGroupRepository dictionaryGroupRepository)
    {
        DictionaryItemRepository = dictionaryItemRepository;
        DictionaryGroupRepository = dictionaryGroupRepository;
    }

    public virtual async Task<ListResultDto<DictionaryItemDto>> GetAllListAsync(DictionaryItemListRequestDto input)
    {
        var list = await DictionaryItemRepository.GetListAsync(filter: input.Filter, groupName: input.GroupName, sorting: nameof(DictionaryItem.Name));

        return new ListResultDto<DictionaryItemDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryItem>, List<DictionaryItemDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DictionaryItemDto>> GetListAsync(DictionaryItemPagedListRequestDto input)
    {
        var count = await DictionaryItemRepository.GetCountAsync(filter: input.Filter, groupName: input.GroupName);
        var list = await DictionaryItemRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, filter: input.Filter, groupName: input.GroupName, sorting: $"{nameof(DictionaryItem.Name)}, {nameof(DictionaryItem.DisplayOrder)} desc");

        return new PagedResultDto<DictionaryItemDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryItem>, List<DictionaryItemDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DictionaryItemDto> GetAsync(string name)
    {
        var entity = await DictionaryItemRepository.GetByNameAsync(name);

        return ObjectMapper.Map<DictionaryItem, DictionaryItemDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryItem.Create)]
    public virtual async Task<DictionaryItemDto> CreateAsync(DictionaryItemCreateDto input)
    {
        var entity = new DictionaryItem(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.GroupName,
            input.DisplayOrder,
            input.IsEnabled,
            input.Description,
            tenantId: CurrentTenant.Id)
        {
            Value = input.Value,
        };

        input.MapExtraPropertiesTo(entity);

        // check name exists
        if (await DictionaryItemRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.NameExists).WithData("name", input.Name);
        }

        // check group exists.
        if (!await DictionaryGroupRepository.IsNameExistsAsync(input.GroupName))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.GroupNotExists).WithData("name", input.GroupName);
        }

        await DictionaryItemRepository.InsertAsync(entity);

        return ObjectMapper.Map<DictionaryItem, DictionaryItemDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryItem.Update)]
    public virtual async Task<DictionaryItemDto> UpdateAsync(string name, DictionaryItemUpdateDto input)
    {
        var entity = await DictionaryItemRepository.GetByNameAsync(name);

        entity.GroupName = input.GroupName;
        entity.DisplayName = input.DisplayName;
        entity.DisplayOrder = input.DisplayOrder;
        entity.IsEnabled = input.IsEnabled;
        entity.Description = input.Description;
        entity.Value = input.Value;

        input.MapExtraPropertiesTo(entity);

        // check group exists.
        if (!await DictionaryGroupRepository.IsNameExistsAsync(input.GroupName))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.GroupNotExists).WithData("name", input.GroupName);
        }

        await DictionaryItemRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DictionaryItem, DictionaryItemDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryItem.Delete)]
    public virtual async Task DeleteAsync(string name)
    {
        await DictionaryItemRepository.DeleteAsync(x => x.Name == name);
    }
}
