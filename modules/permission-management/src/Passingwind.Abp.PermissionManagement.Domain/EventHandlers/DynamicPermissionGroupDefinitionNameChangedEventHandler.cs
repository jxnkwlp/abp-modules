using System.Threading.Tasks;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Passingwind.Abp.PermissionManagement.Eto;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Passingwind.Abp.PermissionManagement.EventHandlers;

public class DynamicPermissionGroupDefinitionNameChangedEventHandler :
    IDistributedEventHandler<DynamicPermissionGroupDefinitionNameChangedEto>,
    //
    IUnitOfWorkEnabled,
    ITransientDependency
{
    private readonly DynamicPermissionManager _dynamicPermissionManager;
    private readonly IAbpPermissionManager _abpPermissionManager;

    public DynamicPermissionGroupDefinitionNameChangedEventHandler(DynamicPermissionManager dynamicPermissionManager, IAbpPermissionManager abpPermissionManager)
    {
        _dynamicPermissionManager = dynamicPermissionManager;
        _abpPermissionManager = abpPermissionManager;
    }

    public virtual async Task HandleEventAsync(DynamicPermissionGroupDefinitionNameChangedEto eventData)
    {
        await _dynamicPermissionManager.ChangePermissionGroupDefinitionNameAsync(eventData.OldTargetName, eventData.TargetName, eventData.DisplayName);

        await _abpPermissionManager.ClearCacheAsync();
    }
}
