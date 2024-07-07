using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.PermissionManagement.Options;
using Volo.Abp.Domain.Services;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionManager : DomainService
{
    private readonly IDynamicPermissionGroupDefinitionRepository _dynamicPermissionGroupDefinitionRepository;
    private readonly IDynamicPermissionDefinitionRepository _dynamicPermissionDefinitionRepository;
    private readonly DynamicPermissionOptions _dynamicPermissionOptions;

    private readonly IAbpPermissionManager _abpPermissionManager;

    public DynamicPermissionManager(
        IDynamicPermissionGroupDefinitionRepository dynamicPermissionGroupDefinitionRepository,
        IDynamicPermissionDefinitionRepository dynamicPermissionDefinitionRepository,
        IOptions<DynamicPermissionOptions> dynamicPermissionOptions,
        IAbpPermissionManager abpPermissionManager)
    {
        _dynamicPermissionGroupDefinitionRepository = dynamicPermissionGroupDefinitionRepository;
        _dynamicPermissionDefinitionRepository = dynamicPermissionDefinitionRepository;
        _dynamicPermissionOptions = dynamicPermissionOptions.Value;
        _abpPermissionManager = abpPermissionManager;
    }

    public virtual async Task InitialPermissionGroupDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        // dynamic group
        var sourceGroups = await _dynamicPermissionGroupDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        // db groups
        var targetGroups = await _abpPermissionManager.GetGroupsAsync(cancellationToken: cancellationToken);

        var sourceGroupNames = sourceGroups.Select(x => x.TargetName).ToArray();
        var targetGroupNames = targetGroups.Select(x => x.Name).ToArray();

        foreach (var group in sourceGroups)
        {
            if (targetGroupNames.Any(x => group.TargetName == x))
                continue;

            await CreateOrUpdatePermissionGroupDefinitionAsync(group.TargetName, group.DisplayName, cancellationToken);
        }
    }

    public virtual async Task InitialPermissionItemDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        // dynamic groups
        var sourceGroups = await _dynamicPermissionGroupDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);

        // dynamic items
        var sources = await _dynamicPermissionDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        // db items
        var targets = await _abpPermissionManager.GetItemsAsync(cancellationToken: cancellationToken);

        var sourceNames = sources.Select(x => x.TargetName).ToArray();
        var targetNames = targets.Select(x => x.Name).ToArray();

        foreach (var source in sources)
        {
            if (targets.Any(x => x.Name == source.TargetName))
                continue;

            var group = sourceGroups.Find(x => x.Id == source.GroupId);
            if (group == null)
            {
                Logger.LogWarning("The dynamic permission '{0}' group id '{1}' not found.", source.Name, source.GroupId);
                continue;
            }

            var groupName = group.TargetName;

            string? parentName = null;
            if (source.ParentId.HasValue)
            {
                var parent = sources.Find(x => x.Id == source.ParentId);
                if (parent != null)
                    parentName = parent.TargetName;
            }

            if (!targets.Any(x => x.Name == source.TargetName && x.GroupName == groupName))
            {
                await CreateOrUpdatePermissionDefinitionAsync(source.TargetName, source.DisplayName, groupName, parentName, source.IsEnabled, cancellationToken);
            }
        }
    }

    public virtual Task<string> NormalizeNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dynamicPermissionOptions.PermissionNamePrefix + name);
    }

    public virtual async Task ChangePermissionGroupDefinitionNameAsync(
        string oldName,
        string newName,
        string displayName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(oldName))
        {
            throw new System.ArgumentException($"'{nameof(oldName)}' cannot be null or empty.", nameof(oldName));
        }

        if (string.IsNullOrEmpty(newName))
        {
            throw new System.ArgumentException($"'{nameof(newName)}' cannot be null or empty.", nameof(newName));
        }

        if (string.IsNullOrEmpty(displayName))
        {
            throw new System.ArgumentException($"'{nameof(displayName)}' cannot be null or empty.", nameof(displayName));
        }

        // 1. update group
        var groups = await _abpPermissionManager.GetGroupsAsync(x => x.Name == oldName, cancellationToken: cancellationToken);

        foreach (var group in groups)
        {
            group.Name = newName;
            group.DisplayName = displayName;

            await _abpPermissionManager.UpdateGroupAsync(group, cancellationToken: cancellationToken);
        }

        // 2. update definition
        var records = await _abpPermissionManager.GetItemsAsync(x => x.GroupName == oldName, cancellationToken: cancellationToken);

        foreach (var item in records)
        {
            item.GroupName = newName;

            await _abpPermissionManager.UpdateItemAsync(item, cancellationToken: cancellationToken);
        }
    }

    public virtual async Task<PermissionGroupDefinitionRecord> CreateOrUpdatePermissionGroupDefinitionAsync(string name, string displayName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(displayName))
        {
            throw new System.ArgumentException($"'{nameof(displayName)}' cannot be null or empty.", nameof(displayName));
        }

        var group = await _abpPermissionManager.FindGroupAsync(x => x.Name == name, cancellationToken: cancellationToken);
        if (group == null)
        {
            return await _abpPermissionManager.CreateGroupAsync(new PermissionGroupDefinitionRecord(GuidGenerator.Create(), name, displayName), cancellationToken: cancellationToken);
        }
        else if (group.DisplayName != displayName)
        {
            group.DisplayName = displayName;
            return await _abpPermissionManager.UpdateGroupAsync(group, cancellationToken: cancellationToken);
        }

        return group;
    }

    public virtual async Task<PermissionDefinitionRecord> CreateOrUpdatePermissionDefinitionAsync(string name, string displayName, string groupName, string? parentName = null, bool isEnabled = true, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(displayName))
        {
            throw new System.ArgumentException($"'{nameof(displayName)}' cannot be null or empty.", nameof(displayName));
        }

        if (string.IsNullOrEmpty(groupName))
        {
            throw new System.ArgumentException($"'{nameof(groupName)}' cannot be null or empty.", nameof(groupName));
        }

        // we do not check params 'ParentName', because 'ParentName' is not real parent.
        var record = await _abpPermissionManager.FindItemAsync(x => x.Name == name && x.GroupName == groupName, cancellationToken: cancellationToken);

        if (record == null)
        {
            record = new PermissionDefinitionRecord(
                id: GuidGenerator.Create(),
                groupName: groupName,
                name: name,
                parentName: parentName,
                displayName: displayName,
                isEnabled: isEnabled);

            await _abpPermissionManager.CreateItemAsync(record, cancellationToken: cancellationToken);
        }
        else
        {
            record.DisplayName = displayName;
            record.ParentName = parentName;
            record.IsEnabled = isEnabled;

            await _abpPermissionManager.UpdateItemAsync(record, cancellationToken: cancellationToken);
        }

        return record;
    }

    public virtual async Task<PermissionGroupDefinitionRecord?> FindPermissionGroupDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        return await _abpPermissionManager.FindGroupAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionDefinitionRecord?> FindPermissionDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        return await _abpPermissionManager.FindItemAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task DeletePermissionDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        if (_dynamicPermissionOptions.AutoCleanPermissions)
            await _abpPermissionManager.DeleteItemsAsync(new string[] { name }, cancellationToken: cancellationToken);
    }

    public virtual async Task DeletePermissionGroupDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        if (_dynamicPermissionOptions.AutoCleanPermissions)
            await _abpPermissionManager.DeleteGroupsAsync(new string[] { name }, cancellationToken: cancellationToken);
    }
}
