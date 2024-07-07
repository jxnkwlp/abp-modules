using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

/// <summary>
///  The dictionary data manager
/// </summary>
public interface IDictionaryManager : IDomainService
{
    /// <summary>
    ///  Get items
    /// </summary>
    Task<IReadOnlyList<DictionaryItemDescriptor>> GetItemsAsync(string groupName, bool onlyEnabled = true, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get groups
    /// </summary>
    Task<IReadOnlyList<DictionaryGroupDescriptor>> GetGroupsAsync(string? parentName = null, bool onlyPublic = true, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Get item value by name
    /// </summary>
    Task<string?> GetItemValueAsync(string itemName, string? defaultValue = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  set an item value by name
    /// </summary>
    Task SetItemValueAsync(string name, string value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set item is enabled
    /// </summary>
    Task SetItemEnabledAsync(string name, bool isEnabled, CancellationToken cancellationToken = default);
    /// <summary>
    /// Set group is public
    /// </summary>
    Task SetGroupPublicAsync(string name, bool isPublic, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Get item descriptor by name
    /// </summary>
    Task<DictionaryItemDescriptor> GetItemAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get group descriptor by name
    /// </summary>
    Task<DictionaryGroupDescriptor> GetGroupAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Check group is exists by name
    /// </summary>
    Task<bool> IsGroupExistsAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Check item is exists by name
    /// </summary>
    Task<bool> IsItemExistsAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create item
    /// </summary>
    Task<DictionaryItem> CreateItemAsync(string name, string displayName, string? value = null, string? description = null, string? groupName = null, int displayOrder = 100, bool isEnabled = true, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Create group
    /// </summary>
    Task<DictionaryGroup> CreateGroupAsync(string name, string displayName, string? parentName = null, string? description = null, bool isPublic = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Delete item by name
    /// </summary>
    Task DeleteItemAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// Delete group by name
    /// </summary>
    Task DeleteGroupAsync(string name, bool removeAllItems = true, CancellationToken cancellationToken = default);
}
