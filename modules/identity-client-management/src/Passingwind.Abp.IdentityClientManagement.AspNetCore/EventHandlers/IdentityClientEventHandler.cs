using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Passingwind.Abp.IdentityClientManagement.Identity;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace Passingwind.Abp.IdentityClientManagement.EventHandlers;

public class IdentityClientEventHandler :
    IDistributedEventHandler<EntityCreatedEto<IdentityClientEto>>,
    IDistributedEventHandler<EntityUpdatedEto<IdentityClientEto>>,
    IDistributedEventHandler<EntityDeletedEto<IdentityClientEto>>,
    ITransientDependency
{
    private readonly ILogger<IdentityClientEventHandler> _logger;
    private readonly IIdentityClientRegisterProvider _identityClientRegisterProvider;
    private readonly IIdentityClientRepository _identityClientRepository;

    public IdentityClientEventHandler(ILogger<IdentityClientEventHandler> logger, IIdentityClientRegisterProvider identityClientRegisterProvider, IIdentityClientRepository identityClientRepository)
    {
        _logger = logger;
        _identityClientRegisterProvider = identityClientRegisterProvider;
        _identityClientRepository = identityClientRepository;
    }

    public async Task HandleEventAsync(EntityCreatedEto<IdentityClientEto> eventData)
    {
        var data = eventData.Entity;

        try
        {
            var entity = await _identityClientRepository.FindAsync(data.Id);
            if (entity != null)
                await _identityClientRegisterProvider.RegisterAsync(entity);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Register identity client provider '{0}' failed.", data.Name);
        }
    }

    public async Task HandleEventAsync(EntityUpdatedEto<IdentityClientEto> eventData)
    {
        var data = eventData.Entity;

        try
        {
            var entity = await _identityClientRepository.FindAsync(eventData.Entity.Id);
            if (entity != null)
            {
                await _identityClientRegisterProvider.UnregisterAsync(entity);
                await _identityClientRegisterProvider.RegisterAsync(entity);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Register identity client provider '{0}' failed.", data.Name);
        }
    }

    public async Task HandleEventAsync(EntityDeletedEto<IdentityClientEto> eventData)
    {
        var data = eventData.Entity;

        try
        {
            var entity = await _identityClientRepository.FindAsync(eventData.Entity.Id);
            if (entity != null)
                await _identityClientRegisterProvider.UnregisterAsync(entity);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Unregister identity client provider '{0}' failed.", data.Name);
        }
    }
}
