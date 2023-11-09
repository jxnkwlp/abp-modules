using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public class FileNameDuplicateDetectionProvider : IFileDuplicateDetectionProvider
{
    private readonly IFileRepository _fileRepository;

    public FileNameDuplicateDetectionProvider(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public virtual async Task<bool> IsExistsAsync(FileContainer fileContainer, File file, CancellationToken cancellationToken = default)
    {
        return await _fileRepository.AnyAsync(x => x.Id != file.Id
                         && x.FileName == file.FileName
                         && x.ParentId == file.ParentId
                         && x.ContainerId == file.ContainerId
                         && x.IsDirectory == file.IsDirectory,
                         cancellationToken);
    }
}
