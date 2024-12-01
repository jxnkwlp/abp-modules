using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.EventHandler;

public class FileEventHander :
    ILocalEventHandler<EntityCreatedEventData<FileItem>>,
    ILocalEventHandler<EntityDeletedEventData<FileItem>>,
    ITransientDependency
{
    private readonly IFileContainerRepository _fileContainerRepository;

    public FileEventHander(IFileContainerRepository fileContainerRepository)
    {
        _fileContainerRepository = fileContainerRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEventData<FileItem> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
            return;

        var container = await _fileContainerRepository.FindAsync(entity.ContainerId);

        if (container != null)
            await _fileContainerRepository.IncrementFileCountAsync(container.Name, 1);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<FileItem> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
            return;

        var container = await _fileContainerRepository.FindAsync(entity.ContainerId);

        if (container?.FilesCount > 0)
            await _fileContainerRepository.IncrementFileCountAsync(container.Name, -1);
    }
}
