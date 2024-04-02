using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryManager : DomainService, IDictionaryManager
{
    private static readonly Regex NameValidRegex = new("^[A-Za-z0-9_\\-\\.]+$");

    protected IDictionaryItemRepository DictionaryItemRepository { get; }
    protected IDictionaryGroupRepository DictionaryGroupRepository { get; }

    public DictionaryManager(IDictionaryItemRepository dictionaryItemRepository, IDictionaryGroupRepository dictionaryGroupRepository)
    {
        DictionaryItemRepository = dictionaryItemRepository;
        DictionaryGroupRepository = dictionaryGroupRepository;
    }

    public static bool IsNameValid(string name)
    {
        return NameValidRegex.IsMatch(name);
    }

    public virtual async Task<string?> GetItemValueAsync(string itemName, string? defaultValue = default, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryItemRepository.FindByNameAsync(itemName, cancellationToken);

        return entity?.Value ?? defaultValue;
    }

    public virtual async Task<IReadOnlyList<DictionaryItemDescriptor>> GetItemsAsync(string groupName, bool onlyEnabled = true, CancellationToken cancellationToken = default)
    {
        var list = await DictionaryItemRepository.GetListAsync(groupName: groupName, isEnabled: onlyEnabled ? true : null, cancellationToken: cancellationToken);

        return list.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).Select(x => new DictionaryItemDescriptor()
        {
            Name = x.Name,
            Description = x.Description,
            DisplayName = x.DisplayName,
            DisplayOrder = x.DisplayOrder,
            GroupName = x.GroupName,
            IsEnabled = x.IsEnabled,
            Value = x.Value,
        }).ToList();
    }

    public virtual async Task<IReadOnlyList<DictionaryGroupDescriptor>> GetGroupsAsync(string? parentName = null, bool onlyPublic = true, CancellationToken cancellationToken = default)
    {
        var list = await DictionaryGroupRepository.GetListAsync(parentName: parentName, isPublic: onlyPublic ? true : null, cancellationToken: cancellationToken);

        return list.OrderBy(x => x.Name).Select(x => new DictionaryGroupDescriptor()
        {
            Name = x.Name,
            Description = x.Description,
            DisplayName = x.DisplayName,
            IsPublic = x.IsPublic,
            ParentName = x.ParentName,
        }).ToList();
    }

    public virtual async Task<DictionaryItemDescriptor> GetItemAsync(string name, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryItemRepository.GetByNameAsync(name, cancellationToken);

        return new DictionaryItemDescriptor
        {
            Name = entity.Name,
            Description = entity.Description,
            DisplayName = entity.DisplayName,
            DisplayOrder = entity.DisplayOrder,
            GroupName = entity.GroupName,
            IsEnabled = entity.IsEnabled,
            Value = entity.Value,
        };
    }

    public virtual async Task<DictionaryGroupDescriptor> GetGroupAsync(string name, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryGroupRepository.GetByNameAsync(name, cancellationToken);

        return new DictionaryGroupDescriptor
        {
            Name = entity.Name,
            Description = entity.Description,
            DisplayName = entity.DisplayName,
            IsPublic = entity.IsPublic,
            ParentName = entity.ParentName,
        };
    }

    public virtual async Task SetItemValueAsync(string name, string value, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryItemRepository.GetByNameAsync(name, cancellationToken);

        entity.Value = value;

        await DictionaryItemRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task SetItemEnabledAsync(string name, bool isEnabled, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryItemRepository.GetByNameAsync(name, cancellationToken);

        if (entity.IsEnabled == isEnabled)
            return;

        entity.IsEnabled = isEnabled;

        await DictionaryItemRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task SetGroupPublicAsync(string name, bool isPublic, CancellationToken cancellationToken = default)
    {
        var entity = await DictionaryGroupRepository.GetByNameAsync(name, cancellationToken: cancellationToken);

        if (entity.IsPublic == isPublic)
            return;

        entity.IsPublic = isPublic;

        await DictionaryGroupRepository.UpdateAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsItemExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DictionaryItemRepository.IsNameExistsAsync(name, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsGroupExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DictionaryGroupRepository.IsNameExistsAsync(name, cancellationToken: cancellationToken);
    }

    public virtual async Task<DictionaryItem> CreateItemAsync(string name, string displayName, string? value = null, string? description = null, string? groupName = null, int displayOrder = 100, bool isEnabled = true, CancellationToken cancellationToken = default)
    {
        if (await DictionaryItemRepository.IsNameExistsAsync(name))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.NameExists).WithData("name", name);
        }

        var entity = new DictionaryItem(
            GuidGenerator.Create(),
            name: name,
            displayName: displayName,
            groupName: groupName,
            displayOrder: displayOrder,
            isEnabled: isEnabled,
            description: description)
        {
            Value = value,
        };

        return await DictionaryItemRepository.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task<DictionaryGroup> CreateGroupAsync(string name, string displayName, string? parentName = null, string? description = null, bool isPublic = false, CancellationToken cancellationToken = default)
    {
        if (await DictionaryGroupRepository.IsNameExistsAsync(name))
        {
            throw new BusinessException(DictionaryManagementErrorCodes.NameExists).WithData("name", name);
        }

        var entity = new DictionaryGroup(
            GuidGenerator.Create(),
            name: name,
            displayName: displayName,
            parentName: parentName,
            description: description,
            isPublic: isPublic);

        return await DictionaryGroupRepository.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteItemAsync(string name, CancellationToken cancellationToken = default)
    {
        await DictionaryItemRepository.DeleteAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteGroupAsync(string name, bool removeAllItems = true, CancellationToken cancellationToken = default)
    {
        await DictionaryGroupRepository.DeleteAsync(x => x.Name == name, cancellationToken: cancellationToken);

        if (removeAllItems)
        {
            await DictionaryItemRepository.DeleteAsync(x => x.GroupName == name, cancellationToken: cancellationToken);
        }
    }
}
