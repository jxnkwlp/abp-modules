using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.DictionaryManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

[Authorize(DictionaryManagementPermissions.DictionaryGroup.Default)]
public class DictionaryGroupAppService : DictionaryManagementAppService, IDictionaryGroupAppService
{
    private readonly IDictionaryGroupRepository _dictionaryGroupRepository;

    public DictionaryGroupAppService(IDictionaryGroupRepository dictionaryGroupRepository)
    {
        _dictionaryGroupRepository = dictionaryGroupRepository;
    }

    public virtual async Task<ListResultDto<DictionaryGroupDto>> GetAllListAsync()
    {
        var list = await _dictionaryGroupRepository.GetListAsync();

        return new ListResultDto<DictionaryGroupDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryGroup>, List<DictionaryGroupDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DictionaryGroupDto>> GetListAsync(DictionaryGroupListRequestDto input)
    {
        var count = await _dictionaryGroupRepository.GetCountAsync(filter: input.Filter, parentName: input.ParentName);
        var list = await _dictionaryGroupRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, filter: input.Filter, parentName: input.ParentName);

        return new PagedResultDto<DictionaryGroupDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryGroup>, List<DictionaryGroupDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DictionaryGroupDto> GetAsync(string name)
    {
        var entity = await _dictionaryGroupRepository.GetByNameAsync(name);

        return ObjectMapper.Map<DictionaryGroup, DictionaryGroupDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryGroup.Create)]
    public virtual async Task<DictionaryGroupDto> CreateAsync(DictionaryGroupCreateDto input)
    {
        var entity = new DictionaryGroup(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.ParentName,
            input.Description,
            input.IsPublic,
            tenantId: CurrentTenant.Id);

        input.MapExtraPropertiesTo(entity);

        if (await _dictionaryGroupRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.NameExists).WithData("name", input.Name);
        }

        if (!string.IsNullOrWhiteSpace(input.ParentName))
        {
            var parent = await _dictionaryGroupRepository.FindByNameAsync(input.ParentName);
            if (parent == null)
            {
                throw new BusinessException(DictionaryManagementErrorCodes.GroupNotExists).WithData("name", input.ParentName);
            }
        }

        await _dictionaryGroupRepository.InsertAsync(entity);

        return ObjectMapper.Map<DictionaryGroup, DictionaryGroupDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryGroup.Update)]
    public virtual async Task<DictionaryGroupDto> UpdateAsync(string name, DictionaryGroupUpdateDto input)
    {
        var entity = await _dictionaryGroupRepository.GetByNameAsync(name);

        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;
        entity.IsPublic = input.IsPublic;

        input.MapExtraPropertiesTo(entity);

        await _dictionaryGroupRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DictionaryGroup, DictionaryGroupDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryGroup.Delete)]
    public virtual async Task DeleteAsync(string name)
    {
        await _dictionaryGroupRepository.DeleteAsync(x => x.Name == name);
    }
}
