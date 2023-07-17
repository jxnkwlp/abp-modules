using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.FileManagement.Files;

public class FileInfoCheckProvider : IFileInfoCheckProvider
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly IFileDuplicateDetectionProvider _fileDuplicateDetectionProvider;

    public FileInfoCheckProvider(IFileRepository fileRepository, IFileContainerRepository fileContainerRepository, IFileDuplicateDetectionProvider fileDuplicateDetectionProvider)
    {
        _fileRepository = fileRepository;
        _fileContainerRepository = fileContainerRepository;
        _fileDuplicateDetectionProvider = fileDuplicateDetectionProvider;
    }

    public async Task CheckAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
    {
        if (!entity.IsDirectory)
        {
            // check file extensions
            await CheckFileExtensionAsync(container, entity, cancellationToken);

            // check file extensions
            await CheckFileSizeAsync(container, entity, cancellationToken);

            // check file exists
            await CheckFileExistsAsync(container, entity, cancellationToken);

            // check container file quantity
            await CheckContainerFileTotalQuantitiesAsync(container, entity, cancellationToken);
        }
        else
        {
            await CheckDirectoryExistsAsync(container, entity, cancellationToken);
        }
    }

    protected virtual Task CheckFileExtensionAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return Task.CompletedTask;

        // check file extensions
        var fileExtension = Path.GetExtension(file.FileName);

        if (container.AllowAnyFileExtension && container.GetProhibitedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) == true)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExtensionNotAllowed).WithData("ext", fileExtension);
        }

        if (!container.AllowAnyFileExtension && container.GetAllowedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) != true)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExtensionNotAllowed).WithData("ext", fileExtension);
        }

        return Task.CompletedTask;
    }

    protected virtual Task CheckFileSizeAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return Task.CompletedTask;

        // check size
        if (container.MaximumEachFileSize < file.Length)
        {
            throw new BusinessException(FileManagementErrorCodes.FileLengthTooLarge).WithData("size", file.Length);
        }

        return Task.CompletedTask;
    }

    protected virtual async Task CheckFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return;

        if (container.OverrideBehavior != FileOverrideBehavior.None)
            return;

        // check override
        var exist = await IsFileExistsAsync(container, file, cancellationToken);

        if (exist)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", file.FileName);
        }
    }

    protected virtual async Task CheckDirectoryExistsAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
    {
        if (!entity.IsDirectory)
            return;

        // check override
        var exist = await IsDirectoryExistsAsync(container, entity, cancellationToken);

        if (exist)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", entity.FileName);
        }
    }

    protected virtual async Task CheckContainerFileTotalQuantitiesAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
    {
        var filesCount = await _fileRepository.LongCountAsync(x => x.ContainerId == container.Id && !x.IsDirectory);

        if (container.MaximumFileQuantity < filesCount)
        {
            throw new BusinessException(FileManagementErrorCodes.ContainerFileQuantitiesMaximumSurpass).WithData("fileName", entity.FileName);
        }
    }

    protected async Task<bool> IsFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        return await _fileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
    }

    protected async Task<bool> IsDirectoryExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        return await _fileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
    }
}