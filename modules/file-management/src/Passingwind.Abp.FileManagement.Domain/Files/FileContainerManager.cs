using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerManager : DomainService
{
    private readonly IFileContainerRepository _fileContainerRepository;

    public FileContainerManager(IFileContainerRepository fileContainerRepository)
    {
        _fileContainerRepository = fileContainerRepository;
    }

    public virtual async Task<bool> IsExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        return await _fileContainerRepository.CheckExistsAsync(fileContainer, cancellationToken);
    }

    public virtual async Task CheckExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        _ = await _fileContainerRepository.GetByNameAsync(name, cancellationToken);
    }
}
