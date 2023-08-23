using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryAppService : DictionaryManagementAppService, IDictionaryAppService
{
    private readonly IDictionaryGroupRepository _dictionaryGroupRepository;
    private readonly IDictionaryItemRepository _dictionaryItemRepository;

    public DictionaryAppService(IDictionaryGroupRepository dictionaryGroupRepository, IDictionaryItemRepository dictionaryItemRepository)
    {
        _dictionaryGroupRepository = dictionaryGroupRepository;
        _dictionaryItemRepository = dictionaryItemRepository;
    }

    public async Task<DictionaryResultDto> GetAsync(string name)
    {
        var item = await _dictionaryItemRepository.GetByNameAsync(name);

        var group = await _dictionaryGroupRepository.GetByNameAsync(item.GroupName!);

        if (!group.IsPublic && !CurrentUser.IsAuthenticated)
        {
            throw new AbpAuthorizationException();
        }

        if (!item.IsEnabled)
        {
            throw new DictionaryItemDisabledException();
        }

        return ObjectMapper.Map<DictionaryItem, DictionaryResultDto>(item);
    }

    public async Task<DictionaryListResultDto> GetListByGroupAsync(string groupName)
    {
        var group = await _dictionaryGroupRepository.GetByNameAsync(groupName);

        if (!group.IsPublic && !CurrentUser.IsAuthenticated)
        {
            throw new AbpAuthorizationException();
        }

        var list = await _dictionaryItemRepository.GetListAsync(groupName: groupName, isEnabled: true);

        list = list.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToList();

        return new DictionaryListResultDto(ObjectMapper.Map<List<DictionaryItem>, List<DictionaryResultDto>>(list));
    }
}
