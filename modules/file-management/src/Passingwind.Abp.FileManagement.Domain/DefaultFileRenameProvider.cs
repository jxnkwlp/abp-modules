using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileRenameProvider : IFileRenameProvider, ITransientDependency
{
    protected IFileRepository FileRepository { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileMimeTypeProvider FileMimeTypeProvider { get; }

    public DefaultFileRenameProvider(IFileRepository fileRepository, IFileContainerRepository fileContainerRepository, IFileMimeTypeProvider fileMimeTypeProvider)
    {
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        FileMimeTypeProvider = fileMimeTypeProvider;
    }

    public virtual async Task<string> RenameAsync(string container, string fileName, Guid? parentId = null, bool isDirectory = false, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await RenameAsync(fileContainer, fileName, parentId, isDirectory, cancellationToken);
    }

    public virtual async Task<string> RenameAsync(FileContainer container, string fileName, Guid? parentId = null, bool isDirectory = false, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var name = Path.GetFileNameWithoutExtension(fileName);
        var ext = Path.GetExtension(fileName);
        var mimeType = FileMimeTypeProvider.Get(fileName);

        if (parentId == null)
        {
            parentId = Guid.Empty;
        }

        bool fileExists = false;

        do
        {
            fileExists = await FileRepository.IsFileNameExistsAsync(containerId: container.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);

            if (!fileExists)
            {
                break;
            }

            name = $"{name} - renamed";

            var count = await FileRepository.CountAsync(x => x.ContainerId == container.Id
                && x.IsDirectory == isDirectory
                && x.ParentId == parentId
                && x.MimeType == mimeType
                && x.FileName.StartsWith(name), cancellationToken: cancellationToken);

            fileName = $"{name} ({count + 1}){ext}";
        } while (fileExists);

        return fileName;
    }
}
