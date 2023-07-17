using System.Threading.Tasks;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.EventHandler;

public class FileCreateEventHander : ILocalEventHandler<EntityCreatedEventData<File>>, ITransientDependency
{
    private readonly IFileContainerRepository _fileContainerRepository;

    public FileCreateEventHander(IFileContainerRepository fileContainerRepository)
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

        if (container != null)
        {
            container.SetFilesCount(container.FilesCount + 1);
        }
    }
}
