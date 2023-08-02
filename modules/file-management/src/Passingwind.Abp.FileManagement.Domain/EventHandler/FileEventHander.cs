using System.Threading.Tasks;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.EventHandler;

public class FileEventHander :
    ILocalEventHandler<EntityCreatedEventData<File>>,
    ILocalEventHandler<EntityDeletedEventData<File>>,
    ITransientDependency
{
    private readonly IFileContainerRepository _fileContainerRepository;

    public FileEventHander(IFileContainerRepository fileContainerRepository)
    {
        _fileContainerRepository = fileContainerRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEventData<File> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
            return;

        var container = await _fileContainerRepository.FindAsync(entity.ContainerId);

        container?.SetFilesCount(container.FilesCount + 1);
    }

    [UnitOfWork]
    public async Task HandleEventAsync(EntityDeletedEventData<File> eventData)
    {
        var entity = eventData.Entity;

        if (entity.IsDirectory)
            return;

        var container = await _fileContainerRepository.FindAsync(entity.ContainerId);

        if (container?.FilesCount > 0)
            container?.SetFilesCount(container.FilesCount - 1);
    }
}
