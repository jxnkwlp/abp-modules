using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passingwind.Abp.DynamicPermissionManagement.Options;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionManager : DomainService
{
    private readonly IDynamicPermissionGroupDefinitionRepository _dynamicPermissionGroupDefinitionRepository;
    private readonly IDynamicPermissionDefinitionRepository _dynamicPermissionDefinitionRepository;
    private readonly DynamicPermissionManagementOptions _dynamicPermissionOptions;

    private readonly IPermissionGroupDefinitionRecordRepository _permissionGroupDefinitionRecordRepository;
    private readonly IPermissionDefinitionRecordRepository _permissionDefinitionRecordRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly AbpDistributedCacheOptions _cacheOptions;

    public DynamicPermissionManager(
        IDynamicPermissionGroupDefinitionRepository dynamicPermissionGroupDefinitionRepository,
        IDynamicPermissionDefinitionRepository dynamicPermissionDefinitionRepository,
        IOptions<DynamicPermissionManagementOptions> dynamicPermissionOptions,
        IPermissionGroupDefinitionRecordRepository permissionGroupDefinitionRecordRepository,
        IPermissionDefinitionRecordRepository permissionDefinitionRecordRepository,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions)
    {
        _dynamicPermissionGroupDefinitionRepository = dynamicPermissionGroupDefinitionRepository;
        _dynamicPermissionDefinitionRepository = dynamicPermissionDefinitionRepository;
        _dynamicPermissionOptions = dynamicPermissionOptions.Value;
        _permissionGroupDefinitionRecordRepository = permissionGroupDefinitionRecordRepository;
        _permissionDefinitionRecordRepository = permissionDefinitionRecordRepository;
        _distributedCache = distributedCache;
        _cacheOptions = cacheOptions.Value;
    }

    public virtual async Task InitialGroupDefinitionsToPermissionAsync(CancellationToken cancellationToken = default)
    {
        // 1. group definition
        var sourceGroups = await _dynamicPermissionGroupDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        var targetGroups = await _permissionGroupDefinitionRecordRepository.GetListAsync(cancellationToken: cancellationToken);

        var sourceGroupNames = sourceGroups.Select(x => FormatName(x.Name)).ToArray();
        var targetGroupNames = targetGroups.Where(x => x.Name.StartsWith(_dynamicPermissionOptions.PermissionNamePrefix)).Select(x => x.Name).ToArray();

        if (_dynamicPermissionOptions.AutoCleanPermissions)
        {
            // delete when definition not exists.
            var deleteNames = targetGroupNames.Except(sourceGroupNames);
            if (deleteNames.Any())
                await _permissionGroupDefinitionRecordRepository.DeleteDirectAsync(x => deleteNames.Contains(x.Name));
        }

        foreach (var group in sourceGroups)
        {
            if (targetGroupNames.Any(x => FormatName(group.Name) == x))
                continue;

            await CreateOrUpdateOrGetPermissionGroupDefinitionAsync(group.Name, group.DisplayName);
        }
    }

    public virtual async Task InitialDefinitionsToPermissionAsync(CancellationToken cancellationToken = default)
    {
        // 2. permission definition
        var sourceGroups = await _dynamicPermissionGroupDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);

        var sources = await _dynamicPermissionDefinitionRepository.GetListAsync(cancellationToken: cancellationToken);
        var targets = await _permissionDefinitionRecordRepository.GetListAsync(cancellationToken: cancellationToken);

        var sourceNames = sources.Select(x => FormatName(x.Name)).ToArray();
        var targetNames = targets.Where(x => x.Name.StartsWith(_dynamicPermissionOptions.PermissionNamePrefix)).Select(x => x.Name).ToArray();

        if (_dynamicPermissionOptions.AutoCleanPermissions)
        {
            // delete when definition not exists.
            var deleteNames = targetNames.Except(sourceNames);
            if (deleteNames.Any())
                await _permissionDefinitionRecordRepository.DeleteDirectAsync(x => deleteNames.Contains(x.Name));
        }

        foreach (var source in sources)
        {
            var name = FormatName(source.Name);

            if (targets.Any(x => x.Name == name))
                continue;

            var group = sourceGroups.Find(x => x.Id == source.GroupId);
            if (group == null)
            {
                Logger.LogWarning("The dynamic permission '{0}' group id '{1}' not found.", source.Name, source.GroupId);
                continue;
            }
            var groupName = FormatName(group.Name);

            string? parentName = null;
            if (source.ParentId.HasValue)
            {
                var parent = sources.Find(x => x.Id == source.ParentId);
                if (parent != null)
                    parentName = FormatName(parent.Name);
            }

            if (!targets.Any(x => x.Name == name && x.GroupName == groupName))
            {
                await CreateOrUpdatePermissionDefinitionAsync(name, source.DisplayName, groupName, parentName, source.IsEnabled);
            }
        }
    }

    public virtual string FormatName(string name)
    {
        if (name.StartsWith(_dynamicPermissionOptions.PermissionNamePrefix))
            return name;

        return _dynamicPermissionOptions.PermissionNamePrefix + name;
    }

    public virtual async Task ChangePermissionGroupDefinitionNameAsync(string oldName, string name, DynamicPermissionGroupDefinition groupDefinition, CancellationToken cancellationToken = default)
    {
        oldName = FormatName(oldName);
        var newName = FormatName(name);

        var entity = groupDefinition;

        // 1. update group
        var groups = await _permissionGroupDefinitionRecordRepository.GetListAsync(x => x.Name == oldName, cancellationToken: cancellationToken);

        foreach (var group in groups)
        {
            group.Name = newName;
            group.DisplayName = entity.DisplayName;

            await _permissionGroupDefinitionRecordRepository.UpdateAsync(group, cancellationToken: cancellationToken);
        }

        // 2. update definition
        var records = await _permissionDefinitionRecordRepository.GetListAsync(x => x.GroupName == oldName, cancellationToken: cancellationToken);

        foreach (var item in records)
        {
            item.GroupName = newName;

            await _permissionDefinitionRecordRepository.UpdateAsync(item, cancellationToken: cancellationToken);
        }
    }

    public virtual async Task<PermissionGroupDefinitionRecord> CreateOrUpdateOrGetPermissionGroupDefinitionAsync(string name, string displayName, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);

        var group = await _permissionGroupDefinitionRecordRepository.FindAsync(x => x.Name == name, cancellationToken: cancellationToken);
        if (group == null)
        {
            return await _permissionGroupDefinitionRecordRepository.InsertAsync(new PermissionGroupDefinitionRecord(GuidGenerator.Create(), name, displayName), cancellationToken: cancellationToken);
        }
        else if (group.DisplayName != displayName)
        {
            group.DisplayName = displayName;
            return await _permissionGroupDefinitionRecordRepository.UpdateAsync(group, cancellationToken: cancellationToken);
        }

        return group;
    }

    public virtual async Task<PermissionDefinitionRecord> CreateOrUpdatePermissionDefinitionAsync(string name, string displayName, string groupName, string? parentName = null, bool isEnabled = true, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);
        groupName = FormatName(groupName);
        if (!string.IsNullOrWhiteSpace(parentName))
            parentName = FormatName(parentName!);

        // we do not check params 'ParentName', because 'ParentName' is not real parent.
        var record = await _permissionDefinitionRecordRepository.FindAsync(x => x.Name == name && x.GroupName == groupName);

        if (record == null)
        {
            record = new PermissionDefinitionRecord(
                id: GuidGenerator.Create(),
                groupName: groupName,
                name: name,
                parentName: parentName,
                displayName: displayName,
                isEnabled: isEnabled);

            await _permissionDefinitionRecordRepository.InsertAsync(record, cancellationToken: cancellationToken);
        }
        else
        {
            record.DisplayName = displayName;
            record.ParentName = parentName;
            record.IsEnabled = isEnabled;
            await _permissionDefinitionRecordRepository.UpdateAsync(record, cancellationToken: cancellationToken);
        }

        return record;
    }

    public virtual async Task<PermissionGroupDefinitionRecord> FindPermissionGroupDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);

        return await _permissionGroupDefinitionRecordRepository.FindAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionDefinitionRecord> FindPermissionDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);

        return await _permissionDefinitionRecordRepository.FindAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task DeletePermissionDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);

        if (_dynamicPermissionOptions.AutoCleanPermissions)
            await _permissionDefinitionRecordRepository.DeleteDirectAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task DeletePermissionGroupDefinitionAsync(string name, CancellationToken cancellationToken = default)
    {
        name = FormatName(name);

        if (_dynamicPermissionOptions.AutoCleanPermissions)
            await _permissionGroupDefinitionRecordRepository.DeleteDirectAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    #region Copy from ABP source

    private string GetCommonStampCacheKey()
    {
        return $"{_cacheOptions.KeyPrefix}_AbpInMemoryPermissionCacheStamp";
    }

    public virtual async Task ClearPermissionDefinitionCacheAsync()
    {
        var cacheKey = GetCommonStampCacheKey();
        await _distributedCache.RemoveAsync(cacheKey);
    }

    #endregion Copy from ABP source
}
