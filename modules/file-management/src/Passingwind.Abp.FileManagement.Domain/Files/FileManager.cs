using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement.Files;

public class FileManager : DomainService, IFileManager
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileContainerRepository _fileContainerRepository;
    private readonly IBlobContainerFactory _blobContainerFactor;
    private readonly IFileBlobNameGenerator _fileBlobNameGenerator;
    private readonly IFileHashCalculator _fileHashCalculator;
    private readonly IFileMimeTypeProvider _fileMimeTypeProvider;
    private readonly IFileUniqueIdGenerator _fileUniqueIdGenerator;
    private readonly IFileBlobContainerProvider _fileBlobContainerProvider;
    private readonly IFileDuplicateDetectionProvider _fileDuplicateDetectionProvider;
    private readonly FileManagementOptions _options;

    public FileManager(
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IBlobContainerFactory blobContainerFactor,
        IFileBlobNameGenerator fileBlobNameGenerator,
        IFileHashCalculator fileHashCalculator,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileUniqueIdGenerator fileUniqueIdGenerator,
        IFileBlobContainerProvider fileBlobContainerProvider,
        IFileDuplicateDetectionProvider fileDuplicateDetectionProvider,
        IOptions<FileManagementOptions> options)
    {
        _fileRepository = fileRepository;
        _fileContainerRepository = fileContainerRepository;
        _blobContainerFactor = blobContainerFactor;
        _fileBlobNameGenerator = fileBlobNameGenerator;
        _fileHashCalculator = fileHashCalculator;
        _fileMimeTypeProvider = fileMimeTypeProvider;
        _fileUniqueIdGenerator = fileUniqueIdGenerator;
        _fileBlobContainerProvider = fileBlobContainerProvider;
        _fileDuplicateDetectionProvider = fileDuplicateDetectionProvider;
        _options = options.Value;
    }

    public async Task<File> FindFileAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        return await _fileRepository.FirstOrDefaultAsync(x => x.FileName == fileName && x.ParentId == parentId && x.ContainerId == container.Id && !x.IsDirectory);
    }

    public virtual async Task<bool> IsFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (file.IsDirectory)
            throw new ArgumentException();

        return await _fileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
    }

    public virtual async Task<bool> IsDirectoryExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (!file.IsDirectory)
            throw new ArgumentException();

        return await _fileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
    }

    public virtual Task CheckFileExtensionAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

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

    public virtual Task CheckFileSizeAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (file.IsDirectory)
            return Task.CompletedTask;

        // check size
        if (container.MaximumEachFileSize < file.Length)
        {
            throw new BusinessException(FileManagementErrorCodes.FileLengthTooLarge).WithData("size", file.Length);
        }

        return Task.CompletedTask;
    }

    public virtual async Task CheckFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (file.IsDirectory)
            return;

        // check override
        var exist = await IsFileExistsAsync(container, file, cancellationToken);

        if (exist)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", file.FileName);
        }
    }

    public async Task CheckDirectoryExistsAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (!entity.IsDirectory)
            return;

        // check override
        var exist = await IsDirectoryExistsAsync(container, entity, cancellationToken);

        if (exist)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", entity.FileName);
        }
    }

    public virtual async Task<File> CreateFileAsync(FileContainer container, string fileName, string mimeType, byte[] bytes, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        var hash = await _fileHashCalculator.GetAsync(bytes);

        var fileId = GuidGenerator.Create();
        var uniqueId = await _fileUniqueIdGenerator.CreateAsync(container, fileId);
        var blobName = await _fileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, fileName, mimeType, bytes.Length, hash);

        return new File(fileId, container.Id, false, fileName, mimeType, bytes.Length, blobName, hash, uniqueId);
    }

    public async Task<File> CreateDirectoryAsync(FileContainer container, string name, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        var fileId = GuidGenerator.Create();
        var uniqueId = await _fileUniqueIdGenerator.CreateAsync(container, fileId);
        var blobName = await _fileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, name, string.Empty, 0, string.Empty);

        return new File(fileId, container.Id, true, name, null, 0, blobName, null, uniqueId);
    }

    public Task<File> UpdateFileAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        file.UpdateLength(bytes.Length);

        return Task.FromResult(file);
    }

    public virtual async Task<byte[]> GetFileBytesAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        var blobContainer = await _fileBlobContainerProvider.GetAsync(container);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        var blobContainer = await _fileBlobContainerProvider.GetAsync(container);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task SaveBlobAsync(FileContainer container, File file, Stream stream, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var blobContainer = await _fileBlobContainerProvider.GetAsync(container);

        await blobContainer.SaveAsync(file.BlobName, stream, true, cancellationToken);
    }

    public virtual async Task SaveBlobAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        var blobContainer = await _fileBlobContainerProvider.GetAsync(container);

        await blobContainer.SaveAsync(file.BlobName, bytes, true, cancellationToken);
    }

    public async Task<File> ChangeFileNameAsync(FileContainer container, File file, string newName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container == null)
            throw new ArgumentNullException(nameof(container));

        if (file == null)
            throw new ArgumentNullException(nameof(file));

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException($"'{nameof(newName)}' cannot be null or whitespace.", nameof(newName));
        }

        file.ChangeParentId(parentId);
        file.SetFileName(newName);

        if (!file.IsDirectory)
            file.UpdateMimeType(_fileMimeTypeProvider.Get(newName));

        await CheckFileExtensionAsync(container, file);

        return file;
    }

}
