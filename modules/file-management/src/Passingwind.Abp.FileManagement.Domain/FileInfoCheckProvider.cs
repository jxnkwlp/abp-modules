using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace Passingwind.Abp.FileManagement;

public class FileInfoCheckProvider : IFileInfoCheckProvider
{
    private readonly IFileItemRepository _fileRepository;
    private readonly IFileContainerRepository _fileContainerRepository;

    public FileInfoCheckProvider(IFileItemRepository fileRepository, IFileContainerRepository fileContainerRepository)
    {
        _fileRepository = fileRepository;
        _fileContainerRepository = fileContainerRepository;
    }

    public virtual async Task CheckAsync(FileContainer container, string fileName, string mimeType, long length, CancellationToken cancellationToken = default)
    {
        // check file name
        if (!await CheckFileNameAsync(container, fileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileNameInvalid).WithData("name", fileName);
        }

        // check file extensions
        if (!await CheckFileExtensionAsync(container, fileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileExtensionNotAllowed).WithData("ext", Path.GetExtension(fileName));
        }

        // check file zize
        if (!await CheckFileSizeAsync(container, length, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileLengthTooLarge).WithData("size", length);
        }

        // check container file quantity
        if (!await CheckContainerFileTotalQuantitiesAsync(container, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerFileQuantitiesMaximumSurpass);
        }
    }

    public virtual async Task<bool> IsValidAsync(FileContainer container, string fileName, string mimeType, long length, CancellationToken cancellationToken = default)
    {
        // check file name
        if (!await CheckFileNameAsync(container, fileName, cancellationToken))
        {
            return false;
        }

        // check file extensions
        if (!await CheckFileExtensionAsync(container, fileName, cancellationToken))
        {
            return false;
        }

        // check file extensions
        if (!await CheckFileSizeAsync(container, length, cancellationToken))
        {
            return false;
        }

        // check container file quantity
        return !await CheckContainerFileTotalQuantitiesAsync(container, cancellationToken);
    }

    public Task<bool> IsFileNameValidAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var result = fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        return Task.FromResult(result);
    }

    protected virtual Task<bool> CheckFileNameAsync(FileContainer container, string fileName, CancellationToken cancellationToken = default)
    {
        var result = fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        return Task.FromResult(result);
    }

    protected virtual Task<bool> CheckFileExtensionAsync(FileContainer container, string fileName, CancellationToken cancellationToken = default)
    {
        // check file extensions
        var fileExtension = Path.GetExtension(fileName);

        if (container.AllowAnyFileExtension && container.GetProhibitedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) == true)
        {
            return Task.FromResult(false);
        }

        return !container.AllowAnyFileExtension && container.GetAllowedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) != true
            ? Task.FromResult(false)
            : Task.FromResult(true);
    }

    protected virtual Task<bool> CheckFileSizeAsync(FileContainer container, long length, CancellationToken cancellationToken = default)
    {
        // check size
        return container.MaximumEachFileSize >= length ? Task.FromResult(true) : Task.FromResult(false);
    }

    protected virtual async Task<bool> CheckContainerFileTotalQuantitiesAsync(FileContainer container, CancellationToken cancellationToken = default)
    {
        // check container file quantity
        var filesCount = await _fileRepository.GetCountAsync(containerId: container.Id, isDirectory: false, cancellationToken: cancellationToken);

        return container.MaximumFileQuantity >= filesCount;
    }
}
