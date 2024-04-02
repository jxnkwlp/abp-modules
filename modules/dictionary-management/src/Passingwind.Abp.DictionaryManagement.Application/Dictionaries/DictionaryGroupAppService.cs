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
    protected IDictionaryGroupRepository DictionaryGroupRepository { get; }

    public DictionaryGroupAppService(IDictionaryGroupRepository dictionaryGroupRepository)
    {
        DictionaryGroupRepository = dictionaryGroupRepository;
    }

    public virtual async Task<ListResultDto<DictionaryGroupDto>> GetAllListAsync()
    {
        var list = await DictionaryGroupRepository.GetListAsync();

        return new ListResultDto<DictionaryGroupDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryGroup>, List<DictionaryGroupDto>>(list),
        };
    }

    public virtual async Task<PagedResultDto<DictionaryGroupDto>> GetListAsync(DictionaryGroupListRequestDto input)
    {
        var count = await DictionaryGroupRepository.GetCountAsync(filter: input.Filter, parentName: input.ParentName);
        var list = await DictionaryGroupRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, filter: input.Filter, parentName: input.ParentName);

        return new PagedResultDto<DictionaryGroupDto>()
        {
            Items = ObjectMapper.Map<List<DictionaryGroup>, List<DictionaryGroupDto>>(list),
            TotalCount = count,
        };
    }

    public virtual async Task<DictionaryGroupDto> GetAsync(string name)
    {
        var entity = await DictionaryGroupRepository.GetByNameAsync(name);

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

        if (await DictionaryGroupRepository.IsNameExistsAsync(input.Name))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.NameExists).WithData("name", input.Name);
        }

        if (!string.IsNullOrWhiteSpace(input.ParentName))
        {
            var parent = await DictionaryGroupRepository.FindByNameAsync(input.ParentName);
            if (parent == null)
            {
                throw new BusinessException(DictionaryManagementErrorCodes.GroupNotExists).WithData("name", input.ParentName);
            }
        }

        await DictionaryGroupRepository.InsertAsync(entity);

        return ObjectMapper.Map<DictionaryGroup, DictionaryGroupDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryGroup.Update)]
    public virtual async Task<DictionaryGroupDto> UpdateAsync(string name, DictionaryGroupUpdateDto input)
    {
        var entity = await DictionaryGroupRepository.GetByNameAsync(name);

        entity.DisplayName = input.DisplayName;
        entity.Description = input.Description;
        entity.IsPublic = input.IsPublic;

        input.MapExtraPropertiesTo(entity);

        await DictionaryGroupRepository.UpdateAsync(entity);

        return ObjectMapper.Map<DictionaryGroup, DictionaryGroupDto>(entity);
    }

    [Authorize(DictionaryManagementPermissions.DictionaryGroup.Delete)]
    public virtual async Task DeleteAsync(string name)
    {
        await DictionaryGroupRepository.DeleteAsync(x => x.Name == name);
    }
}
