using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement;

public class AbpPermissionManager : IAbpPermissionManager, IScopedDependency
{
    private readonly IRepository<PermissionGroupDefinitionRecord, Guid> _permissionGroupRepository;
    private readonly IRepository<PermissionDefinitionRecord, Guid> _permissionRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly AbpDistributedCacheOptions _cacheOptions;

    public AbpPermissionManager(
        IRepository<PermissionGroupDefinitionRecord, Guid> permissionGroupRepository,
        IRepository<PermissionDefinitionRecord, Guid> permissionRepository,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions)
    {
        _permissionGroupRepository = permissionGroupRepository;
        _permissionRepository = permissionRepository;
        _distributedCache = distributedCache;
        _cacheOptions = cacheOptions.Value;
    }

    #region Copy from ABP source

    private string GetCommonStampCacheKey()
    {
        return $"{_cacheOptions.KeyPrefix}_AbpInMemoryPermissionCacheStamp";
    }

    protected virtual async Task ClearPermissionDefinitionCacheAsync()
    {
        var cacheKey = GetCommonStampCacheKey();
        await _distributedCache.RemoveAsync(cacheKey);
    }

    #endregion Copy from ABP source

    public virtual async Task ClearCacheAsync(CancellationToken cancellationToken = default)
    {
        await ClearPermissionDefinitionCacheAsync();
    }

    public virtual async Task<PermissionGroupDefinitionRecord> CreateGroupAsync(PermissionGroupDefinitionRecord record, CancellationToken cancellationToken = default)
    {
        return await _permissionGroupRepository.InsertAsync(record, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionDefinitionRecord> CreateItemAsync(PermissionDefinitionRecord record, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.InsertAsync(record, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteGroupsAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        await _permissionGroupRepository.DeleteDirectAsync(x => names.Contains(x.Name), cancellationToken);
    }

    public virtual async Task DeleteItemsAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        await _permissionRepository.DeleteDirectAsync(x => names.Contains(x.Name), cancellationToken);
    }

    public virtual async Task<PermissionGroupDefinitionRecord?> FindGroupAsync(Expression<Func<PermissionGroupDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _permissionGroupRepository.FindAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionDefinitionRecord?> FindItemAsync(Expression<Func<PermissionDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.FindAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<PermissionGroupDefinitionRecord>> GetGroupsAsync(CancellationToken cancellationToken = default)
    {
        return await _permissionGroupRepository.GetListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<PermissionGroupDefinitionRecord>> GetGroupsAsync(Expression<Func<PermissionGroupDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _permissionGroupRepository.GetListAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<PermissionDefinitionRecord>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<PermissionDefinitionRecord>> GetItemsAsync(Expression<Func<PermissionDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.GetListAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionGroupDefinitionRecord> UpdateGroupAsync(PermissionGroupDefinitionRecord record, CancellationToken cancellationToken = default)
    {
        return await _permissionGroupRepository.UpdateAsync(record, cancellationToken: cancellationToken);
    }

    public virtual async Task<PermissionDefinitionRecord> UpdateItemAsync(PermissionDefinitionRecord record, CancellationToken cancellationToken = default)
    {
        return await _permissionRepository.UpdateAsync(record, cancellationToken: cancellationToken);
    }
}
