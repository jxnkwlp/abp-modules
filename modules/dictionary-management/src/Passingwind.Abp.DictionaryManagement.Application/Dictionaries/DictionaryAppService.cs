using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryAppService : DictionaryManagementAppService, IDictionaryAppService
{
    protected IDictionaryManager DictionaryManager { get; }

    public DictionaryAppService(IDictionaryManager dictionaryManager)
    {
        DictionaryManager = dictionaryManager;
    }

    public virtual async Task<DictionaryResultDto> GetAsync(string name)
    {
        var item = await DictionaryManager.GetItemAsync(name);

        if (!string.IsNullOrWhiteSpace(item.GroupName))
        {
            var group = await DictionaryManager.GetGroupAsync(item.GroupName);

            if (!group.IsPublic && !CurrentUser.IsAuthenticated)
            {
                throw new AbpAuthorizationException();
            }
        }

        if (!item.IsEnabled)
        {
            throw new DictionaryItemDisabledException();
        }

        return new DictionaryResultDto
        {
            Description = item.Description,
            DisplayName = item.DisplayName,
            Name = item.Name,
            Value = item.Value,
        };
    }

    public virtual async Task<DictionaryGroupListResultDto> GetGroupListAsync(string? parentName = null)
    {
        bool onlyPublic = true;
        if (CurrentUser.IsAuthenticated)
        {
            onlyPublic = false;
        }

        var list = await DictionaryManager.GetGroupsAsync(parentName: parentName, onlyPublic: onlyPublic);

        return new DictionaryGroupListResultDto(list.Select(x => new DictionaryGroupResultDto
        {
            Description = x.Description,
            DisplayName = x.DisplayName,
            IsPublic = x.IsPublic,
            Name = x.Name,
            ParentName = x.ParentName,
        }).ToList());
    }

    public virtual async Task<DictionaryListResultDto> GetListByGroupAsync(string groupName)
    {
        var group = await DictionaryManager.GetGroupAsync(groupName);

        if (!group.IsPublic && !CurrentUser.IsAuthenticated)
        {
            throw new AbpAuthorizationException();
        }

        var list = await DictionaryManager.GetItemsAsync(groupName: groupName, onlyEnabled: true);

        return new DictionaryListResultDto(list.Select(x => new DictionaryResultDto
        {
            Description = x.Description,
            DisplayName = x.DisplayName,
            Name = x.Name,
            Value = x.Value,
        }).ToList());
    }
}
