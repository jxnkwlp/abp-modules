using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.EventHandlers;

public class FileContainerEventHandler : ILocalEventHandler<EntityDeletedEventData<FileContainer>>, ITransientDependency
{
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }

    public FileContainerEventHandler(IFileAccessTokenRepository fileAccessTokenRepository)
    {
        FileAccessTokenRepository = fileAccessTokenRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<FileContainer> eventData)
    {
        var entity = eventData.Entity;

        // disable default container delete action
        if (entity.GetIsDefault())
        {
            throw new UserFriendlyException("Can't delete default file container");
        }

        // remove file shares 
        await FileAccessTokenRepository.DeleteAsync(x => x.ContainerId == entity.Id);
    }
}
