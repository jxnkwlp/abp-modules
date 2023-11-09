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
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement.Files;

public class FileManager : DomainService, IFileManager
{
    private IFileRepository FileRepository { get; }
    private IFileContainerRepository FileContainerRepository { get; }
    private IBlobContainerFactory BlobContainerFactor { get; }
    private IFileBlobNameGenerator FileBlobNameGenerator { get; }
    private IFileHashCalculator FileHashCalculator { get; }
    private IFileMimeTypeProvider FileMimeTypeProvider { get; }
    private IFileUniqueIdGenerator FileUniqueIdGenerator { get; }
    private IFileBlobContainerProvider FileBlobContainerProvider { get; }
    private IFileDuplicateDetectionProvider FileDuplicateDetectionProvider { get; }
    private FileManagementOptions FileManagementOptions { get; }

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
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        BlobContainerFactor = blobContainerFactor;
        FileBlobNameGenerator = fileBlobNameGenerator;
        FileHashCalculator = fileHashCalculator;
        FileMimeTypeProvider = fileMimeTypeProvider;
        FileUniqueIdGenerator = fileUniqueIdGenerator;
        FileBlobContainerProvider = fileBlobContainerProvider;
        FileDuplicateDetectionProvider = fileDuplicateDetectionProvider;
        FileManagementOptions = options.Value;
    }

    public virtual async Task<File> FindFileAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        return await FileRepository.FirstOrDefaultAsync(x => x.FileName == fileName && x.ParentId == parentId && x.ContainerId == container.Id && !x.IsDirectory);
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

        return await FileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
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

        return await FileDuplicateDetectionProvider.IsExistsAsync(container, file, cancellationToken);
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

    public virtual async Task CheckDirectoryExistsAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
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

    public virtual async Task<File> CreateFileAsync(FileContainer container, string fileName, string mimeType, byte[] bytes, Guid? parentId = null, CancellationToken cancellationToken = default)
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

        var hash = await FileHashCalculator.GetAsync(bytes);

        var fileId = GuidGenerator.Create();
        var uniqueId = await FileUniqueIdGenerator.CreateAsync(container, fileId);
        var blobName = await FileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, fileName, mimeType, bytes.Length, hash);

        var file = new File(
            fileId,
            container.Id,
            isDirectory: false,
            fileName: fileName,
            blobName: blobName,
            mimeType: mimeType,
            length: bytes.Length,
            hash: hash,
            uniqueId: uniqueId);

        file.ChangeParentId(parentId ?? Guid.Empty);

        return file;
    }

    public virtual async Task<File> CreateDirectoryAsync(FileContainer container, string name, Guid? parentId, CancellationToken cancellationToken = default)
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
        var uniqueId = await FileUniqueIdGenerator.CreateAsync(container, fileId);
        var blobName = await FileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, name, string.Empty, 0, string.Empty);

        var file = new File(
            fileId,
            container.Id,
            isDirectory: true,
            fileName: name,
            blobName: blobName,
            mimeType: null,
            length: 0,
            hash: null,
            uniqueId: uniqueId);

        file.ChangeParentId(parentId ?? Guid.Empty);

        return file;
    }

    public virtual Task<File> UpdateFileAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default)
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

        var blobContainer = await FileBlobContainerProvider.GetAsync(container);

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

        var blobContainer = await FileBlobContainerProvider.GetAsync(container);

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

        var blobContainer = await FileBlobContainerProvider.GetAsync(container);

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

        var blobContainer = await FileBlobContainerProvider.GetAsync(container);

        await blobContainer.SaveAsync(file.BlobName, bytes, true, cancellationToken);
    }

    public virtual async Task<File> ChangeNameAsync(FileContainer container, File file, string newName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container == null)
            throw new ArgumentNullException(nameof(container));

        if (file == null)
            throw new ArgumentNullException(nameof(file));

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException($"'{nameof(newName)}' cannot be null or whitespace.", nameof(newName));
        }

        file.ChangeParentId(parentId ?? Guid.Empty);
        file.SetFileName(newName);

        if (!file.IsDirectory)
            file.UpdateMimeType(FileMimeTypeProvider.Get(newName));

        await CheckFileExtensionAsync(container, file);

        return file;
    }

    [UnitOfWork]
    public virtual async Task DeleteAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        await FileRepository.DeleteAsync(file);

        if (container.AutoDeleteBlob && !file.IsDirectory)
        {
            var blobContainer = await FileBlobContainerProvider.GetAsync(container, cancellationToken: cancellationToken);

            await blobContainer.DeleteAsync(file.BlobName, cancellationToken);
        }
    }

    [UnitOfWork]
    public virtual async Task ClearContainerFilesAsync(FileContainer container, CancellationToken cancellationToken = default)
    {
        // TODO: performance

        var files = await FileRepository.GetListAsync(containerId: container.Id, cancellationToken: cancellationToken);

        foreach (var file in files)
        {
            await FileRepository.DeleteAsync(file);

            if (container.AutoDeleteBlob && !file.IsDirectory)
            {
                var blobContainer = await FileBlobContainerProvider.GetAsync(container, cancellationToken: cancellationToken);

                await blobContainer.DeleteAsync(file.BlobName, cancellationToken);
            }
        }
    }

    public virtual async Task<byte[]> GetFileBytesByFileIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName, cancellationToken);
        var file = await FileRepository.GetAsync(id, cancellationToken: cancellationToken);
        return await GetFileBytesAsync(container, file, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamByFileIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName, cancellationToken);
        var file = await FileRepository.GetAsync(id, cancellationToken: cancellationToken);
        return await GetFileSteamAsync(container, file, cancellationToken);
    }

    public virtual async Task<File> GetByIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default)
    {
        return await FileRepository.GetAsync(id, cancellationToken: cancellationToken);
    }
}
