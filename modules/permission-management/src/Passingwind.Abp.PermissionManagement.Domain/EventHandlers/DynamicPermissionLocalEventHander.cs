using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.PermissionManagement.EventHandlers;

public class DynamicPermissionLocalEventHander :
    ILocalEventHandler<EntityCreatedEventData<DynamicPermissionDefinition>>,
    ILocalEventHandler<EntityUpdatedEventData<DynamicPermissionDefinition>>,
    ILocalEventHandler<EntityDeletedEventData<DynamicPermissionDefinition>>,
    //
    ILocalEventHandler<EntityCreatedEventData<DynamicPermissionGroupDefinition>>,
    ILocalEventHandler<EntityDeletedEventData<DynamicPermissionGroupDefinition>>,
    //
    IUnitOfWorkEnabled,
    ITransientDependency
{
    private readonly ILogger<DynamicPermissionLocalEventHander> _logger;
    private readonly IDynamicPermissionGroupDefinitionRepository _permissionGroupDefinitionRepository;
    private readonly IDynamicPermissionDefinitionRepository _permissionDefinitionRepository;
    private readonly DynamicPermissionManager _dynamicPermissionManager;
    private readonly IAbpPermissionManager _abpPermissionManager;

    public DynamicPermissionLocalEventHander(
        ILogger<DynamicPermissionLocalEventHander> logger,
        IDynamicPermissionGroupDefinitionRepository permissionGroupDefinitionRepository,
        IDynamicPermissionDefinitionRepository permissionDefinitionRepository,
        DynamicPermissionManager dynamicPermissionManager,
        IAbpPermissionManager abpPermissionManager)
    {
        _logger = logger;
        _permissionGroupDefinitionRepository = permissionGroupDefinitionRepository;
        _permissionDefinitionRepository = permissionDefinitionRepository;
        _dynamicPermissionManager = dynamicPermissionManager;
        _abpPermissionManager = abpPermissionManager;
    }

    /// <summary>
    ///  Create permission item
    /// </summary>
    public virtual async Task HandleEventAsync(EntityCreatedEventData<DynamicPermissionDefinition> eventData)
    {
        var entity = eventData.Entity;
        var group = await _permissionGroupDefinitionRepository.FindAsync(entity.GroupId);

        if (group == null)
        {
            _logger.LogWarning("The dynamic permission '{0}' group id '{1}' not found.", entity.Name, entity.GroupId);
            return;
        }

        DynamicPermissionDefinition? parent = null;
        if (entity.ParentId.HasValue)
        {
            parent = await _permissionDefinitionRepository.FindAsync(entity.ParentId.Value);
            if (parent == null)
            {
                _logger.LogWarning("The dynamic permission '{0}' parent id '{1}' not found.", entity.Name, entity.ParentId);
                return;
            }
        }

        var groupRecord = await _dynamicPermissionManager.CreateOrUpdatePermissionGroupDefinitionAsync(group.TargetName, group.DisplayName);

        await _dynamicPermissionManager.CreateOrUpdatePermissionDefinitionAsync(
            entity.TargetName,
            entity.DisplayName,
            groupRecord.Name,
            parent?.TargetName,
            entity.IsEnabled);

        await _abpPermissionManager.ClearCacheAsync();
    }

    /// <summary>
    ///  Delete permission item
    /// </summary>
    public virtual async Task HandleEventAsync(EntityDeletedEventData<DynamicPermissionDefinition> eventData)
    {
        var entity = eventData.Entity;

        await _dynamicPermissionManager.DeletePermissionDefinitionAsync(entity.TargetName);

        await _abpPermissionManager.ClearCacheAsync();
    }

    /// <summary>
    /// Update permission item display name
    /// </summary>
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<DynamicPermissionDefinition> eventData)
    {
        var entity = eventData.Entity;
        var group = await _permissionGroupDefinitionRepository.FindAsync(entity.GroupId);

        DynamicPermissionDefinition? parent = null;
        if (entity.ParentId.HasValue)
        {
            parent = await _permissionDefinitionRepository.FindAsync(entity.ParentId.Value);
            if (parent == null)
            {
                _logger.LogWarning("The dynamic permission '{0}' parent id '{1}' not found.", entity.Name, entity.ParentId);
                return;
            }
        }

        var record = await _dynamicPermissionManager.FindPermissionDefinitionAsync(entity.TargetName);

        // if null, maybe changed name
        if (record != null)
        {
            await _dynamicPermissionManager.CreateOrUpdatePermissionDefinitionAsync(
                entity.TargetName,
                entity.DisplayName,
                group.TargetName,
                parent?.TargetName,
                entity.IsEnabled);

            await _abpPermissionManager.ClearCacheAsync();
        }
    }

    /// <summary>
    ///  Create permission group
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual async Task HandleEventAsync(EntityCreatedEventData<DynamicPermissionGroupDefinition> eventData)
    {
        var entity = eventData.Entity;

        await _dynamicPermissionManager.CreateOrUpdatePermissionGroupDefinitionAsync(entity.TargetName, entity.DisplayName);
    }

    /// <summary>
    ///  Delete permission group
    /// </summary>
    public virtual async Task HandleEventAsync(EntityDeletedEventData<DynamicPermissionGroupDefinition> eventData)
    {
        var entity = eventData.Entity;

        // delete group
        await _dynamicPermissionManager.DeletePermissionGroupDefinitionAsync(entity.TargetName);

        // delete items
        await _permissionDefinitionRepository.DeleteDirectAsync(x => x.GroupId == entity.Id);

        // 
        await _abpPermissionManager.ClearCacheAsync();
    }
}
