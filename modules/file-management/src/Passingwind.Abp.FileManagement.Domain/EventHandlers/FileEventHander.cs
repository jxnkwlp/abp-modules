using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.EventHandlers;

public class FileEventHander :
    ILocalEventHandler<EntityCreatedEventData<FileItem>>,
    ILocalEventHandler<EntityDeletedEventData<FileItem>>,
    ITransientDependency
{
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }

    public FileEventHander(IFileContainerRepository fileContainerRepository, IFileAccessTokenRepository fileAccessTokenRepository)
    {
        FileContainerRepository = fileContainerRepository;
        FileAccessTokenRepository = fileAccessTokenRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEventData<FileItem> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
        {
            return;
        }

        var container = await FileContainerRepository.FindAsync(entity.ContainerId);

        if (container != null)
        {
            await FileContainerRepository.IncrementFileCountAsync(container.Name, 1);
        }
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<FileItem> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
        {
            return;
        }

        // remove file shares
        await FileAccessTokenRepository.DeleteAsync(x => x.FileId == entity.Id);

        // update file total count in container
        var container = await FileContainerRepository.FindAsync(entity.ContainerId);

        if (container?.FilesCount > 0)
        {
            await FileContainerRepository.IncrementFileCountAsync(container.Name, -1);
        }
    }
}
