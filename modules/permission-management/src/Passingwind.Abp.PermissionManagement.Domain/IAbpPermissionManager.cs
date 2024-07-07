using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement;

public interface IAbpPermissionManager
{
    Task<List<PermissionGroupDefinitionRecord>> GetGroupsAsync(CancellationToken cancellationToken = default);
    Task<List<PermissionGroupDefinitionRecord>> GetGroupsAsync(Expression<Func<PermissionGroupDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default);
    Task<PermissionGroupDefinitionRecord?> FindGroupAsync(Expression<Func<PermissionGroupDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteGroupsAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<PermissionGroupDefinitionRecord> CreateGroupAsync(PermissionGroupDefinitionRecord record, CancellationToken cancellationToken = default);
    Task<PermissionGroupDefinitionRecord> UpdateGroupAsync(PermissionGroupDefinitionRecord record, CancellationToken cancellationToken = default);

    Task<List<PermissionDefinitionRecord>> GetItemsAsync(CancellationToken cancellationToken = default);
    Task<List<PermissionDefinitionRecord>> GetItemsAsync(Expression<Func<PermissionDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default);
    Task<PermissionDefinitionRecord?> FindItemAsync(Expression<Func<PermissionDefinitionRecord, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteItemsAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<PermissionDefinitionRecord> CreateItemAsync(PermissionDefinitionRecord record, CancellationToken cancellationToken = default);
    Task<PermissionDefinitionRecord> UpdateItemAsync(PermissionDefinitionRecord record, CancellationToken cancellationToken = default);

    Task ClearCacheAsync(CancellationToken cancellationToken = default);
}
