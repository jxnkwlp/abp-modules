using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.DynamicPermissionManagement.Eto;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.DynamicPermissionManagement.EventHandlers;

public class DynamicPermissionDefinitionEventHander :
    ILocalEventHandler<EntityCreatedEventData<DynamicPermissionDefinition>>,
    ILocalEventHandler<EntityUpdatedEventData<DynamicPermissionDefinition>>,
    ILocalEventHandler<EntityDeletedEventData<DynamicPermissionDefinition>>,
    // ILocalEventHandler<DynamicPermissionDefinitionNameChangedEto>,
    ILocalEventHandler<EntityUpdatedEventData<DynamicPermissionGroupDefinition>>,
    ILocalEventHandler<EntityDeletedEventData<DynamicPermissionGroupDefinition>>,
    ILocalEventHandler<DynamicPermissionGroupDefinitionNameChangedEto>,
    IUnitOfWorkEnabled,
    ITransientDependency
{
    private readonly ILogger<DynamicPermissionDefinitionEventHander> _logger;
    private readonly IDynamicPermissionGroupDefinitionRepository _permissionGroupDefinitionRepository;
    private readonly IDynamicPermissionDefinitionRepository _permissionDefinitionRepository;
    private readonly DynamicPermissionManager _dynamicPermissionManager;

    public DynamicPermissionDefinitionEventHander(
        ILogger<DynamicPermissionDefinitionEventHander> logger,
        IDynamicPermissionGroupDefinitionRepository permissionGroupDefinitionRepository,
        IDynamicPermissionDefinitionRepository permissionDefinitionRepository,
        DynamicPermissionManager dynamicPermissionManager)
    {
        _logger = logger;
        _permissionGroupDefinitionRepository = permissionGroupDefinitionRepository;
        _permissionDefinitionRepository = permissionDefinitionRepository;
        _dynamicPermissionManager = dynamicPermissionManager;
    }

    /// <summary>
    ///  DynamicPermissionDefinition Created
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(EntityCreatedEventData<DynamicPermissionDefinition> eventData)
    {
        var definition = eventData.Entity;
        var group = await _permissionGroupDefinitionRepository.FindAsync(definition.GroupId);

        if (group == null)
        {
            _logger.LogWarning("The dynamic permission '{0}' group id '{1}' not found.", definition.Name, definition.GroupId);
            return;
        }

        DynamicPermissionDefinition? parent = null;
        if (definition.ParentId.HasValue)
        {
            parent = await _permissionDefinitionRepository.FindAsync(definition.ParentId.Value);
            if (parent == null)
            {
                _logger.LogWarning("The dynamic permission '{0}' parent id '{1}' not found.", definition.Name, definition.ParentId);
                return;
            }
        }

        var groupRecord = await _dynamicPermissionManager.CreateOrUpdateOrGetPermissionGroupDefinitionAsync(group.Name, group.DisplayName);

        await _dynamicPermissionManager.CreateOrUpdatePermissionDefinitionAsync(definition.Name, definition.DisplayName, groupRecord.Name, parent?.Name, definition.IsEnabled);

        await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
    }

    /// <summary>
    ///  DynamicPermissionDefinition Deleted
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(EntityDeletedEventData<DynamicPermissionDefinition> eventData)
    {
        var entity = eventData.Entity;

        await _dynamicPermissionManager.DeletePermissionDefinitionAsync(entity.Name);

        await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
    }

    /// <summary>
    ///  DynamicPermissionDefinition Updated
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(EntityUpdatedEventData<DynamicPermissionDefinition> eventData)
    {
        var entity = eventData.Entity;

        var record = await _dynamicPermissionManager.FindPermissionDefinitionAsync(entity.Name);

        // if null, maybe changed name
        if (record != null)
        {
            await _dynamicPermissionManager.CreateOrUpdatePermissionDefinitionAsync(record.Name, record.DisplayName, record.GroupName, record.ParentName, record.IsEnabled);

            await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
        }
    }

    /// <summary>
    ///  DynamicPermissionGroupDefinition updated
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(EntityUpdatedEventData<DynamicPermissionGroupDefinition> eventData)
    {
        var entity = eventData.Entity;

        var record = await _dynamicPermissionManager.FindPermissionGroupDefinitionAsync(entity.Name);

        // if null, maybe changed name
        if (record != null)
        {
            await _dynamicPermissionManager.CreateOrUpdateOrGetPermissionGroupDefinitionAsync(entity.Name, entity.DisplayName);

            // 
            await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
        }
    }

    /// <summary>
    ///  DynamicPermissionGroupDefinition deleted
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(EntityDeletedEventData<DynamicPermissionGroupDefinition> eventData)
    {
        var entity = eventData.Entity;

        await _dynamicPermissionManager.DeletePermissionGroupDefinitionAsync(entity.Name);

        await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
    }

    /// <summary>
    ///  DynamicPermissionGroupDefinition changed name
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(DynamicPermissionGroupDefinitionNameChangedEto eventData)
    {
        await _dynamicPermissionManager.ChangePermissionGroupDefinitionNameAsync(eventData.OldName, eventData.Name, eventData.Entity);

        await _dynamicPermissionManager.ClearPermissionDefinitionCacheAsync();
    }
}
