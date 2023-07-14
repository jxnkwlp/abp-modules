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
    private readonly FileManagementOptions _options;

    public FileManager(
        IFileRepository fileRepository,
        IBlobContainerFactory blobContainerFactor,
        IFileBlobNameGenerator fileBlobNameGenerator,
        IFileHashCalculator fileHashCalculator,
        IOptions<FileManagementOptions> options,
        IFileContainerRepository fileContainerRepository,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileUniqueIdGenerator fileUniqueIdGenerator)
    {
        _fileRepository = fileRepository;
        _blobContainerFactor = blobContainerFactor;
        _fileBlobNameGenerator = fileBlobNameGenerator;
        _fileHashCalculator = fileHashCalculator;
        _options = options.Value;
        _fileContainerRepository = fileContainerRepository;
        _fileMimeTypeProvider = fileMimeTypeProvider;
        _fileUniqueIdGenerator = fileUniqueIdGenerator;
    }

    public async Task<File> FindFileAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        return await _fileRepository.FirstOrDefaultAsync(x => x.FileName == fileName && x.ParentId == parentId && x.ContainerId == container.Id && !x.IsDirectory);
    }

    public virtual async Task<bool> IsFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        return await _fileRepository.AnyAsync(x => x.Id != file.Id
            && x.FileName == file.FileName
            && x.ParentId == file.ParentId
            && x.ContainerId == file.ContainerId
            && !x.IsDirectory);
    }

    public virtual async Task<bool> IsDirectoryExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        return await _fileRepository.AnyAsync(x => x.Id != file.Id
            && x.FileName == file.FileName
            && x.ParentId == file.ParentId
            && x.ContainerId == file.ContainerId
            && x.IsDirectory);
    }

    public virtual Task CheckFileExtensionAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return Task.CompletedTask;

        // check file extensions
        var fileExtension = Path.GetExtension(file.FileName);

        if (container.AllowAnyFileExtension && container.GetProhibitedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) == true)
        {
            throw new BusinessException("FileManagement:FileExtensionNotAllowed").WithData("ext", fileExtension);
        }

        if (!container.AllowAnyFileExtension && container.GetAllowedFileExtensions()?.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase) != true)
        {
            throw new BusinessException("FileManagement:FileExtensionNotAllowed").WithData("ext", fileExtension);
        }

        return Task.CompletedTask;
    }

    public virtual Task CheckFileSizeAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return Task.CompletedTask;

        // check size
        if (container.MaximumEachFileSize < file.Length)
        {
            throw new BusinessException("FileManagement:FileLengthTooLarge").WithData("size", file.Length);
        }

        return Task.CompletedTask;
    }

    public virtual async Task CheckFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        if (file.IsDirectory)
            return;

        // check override
        var exist = await IsFileExistsAsync(container, file, cancellationToken);

        if (exist)
        {
            throw new BusinessException("FileManagement:FileExists").WithData("fileName", file.FileName);
        }
    }

    public async Task CheckDirectoryExistsAsync(FileContainer container, File entity, CancellationToken cancellationToken = default)
    {
        if (!entity.IsDirectory)
            return;

        // check override
        var exist = await IsDirectoryExistsAsync(container, entity, cancellationToken);

        if (exist)
        {
            throw new BusinessException("FileManagement:FileExists").WithData("fileName", entity.FileName);
        }
    }

    public virtual async Task<File> CreateFileAsync(FileContainer container, string fileName, string mimeType, byte[] bytes, CancellationToken cancellationToken = default)
    {
        var hash = await _fileHashCalculator.GetAsync(bytes);

        var fileId = GuidGenerator.Create();
        var uniqueId = await _fileUniqueIdGenerator.CreateAsync(container);
        var blobName = await _fileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, fileName, mimeType, bytes.Length, hash);

        return new File(fileId, container.Id, false, fileName, mimeType, bytes.Length, blobName, hash, uniqueId);
    }

    public async Task<File> CreateDirectoryAsync(FileContainer container, string name, Guid? parentId, CancellationToken cancellationToken = default)
    {
        var fileId = GuidGenerator.Create();
        var uniqueId = await _fileUniqueIdGenerator.CreateAsync(container);
        var blobName = await _fileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, name, string.Empty, 0, string.Empty);

        return new File(fileId, container.Id, true, name, null, 0, blobName, null, uniqueId);
    }

    public Task<File> UpdateFileAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default)
    {
        file.UpdateLength(bytes.Length);

        return Task.FromResult(file);
    }

    public virtual async Task<byte[]> GetFileBytesAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        var blobContainer = GetBlobContainer(container);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(FileContainer container, File file, CancellationToken cancellationToken = default)
    {
        var blobContainer = GetBlobContainer(container);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task SaveBlobAsync(FileContainer container, File file, Stream stream, CancellationToken cancellationToken = default)
    {
        var blobContainer = GetBlobContainer(container);

        await blobContainer.SaveAsync(file.BlobName, stream, true, cancellationToken);
    }

    public virtual async Task SaveBlobAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default)
    {
        var blobContainer = GetBlobContainer(container);

        await blobContainer.SaveAsync(file.BlobName, bytes, true, cancellationToken);
    }

    protected virtual IBlobContainer GetBlobContainer(FileContainer container)
    {
        return _blobContainerFactor.Create(_options.DefaultBlobContainer);
    }

    public File ChangeDirectoryName(FileContainer container, File directory, string newName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        if (container == null)
            throw new ArgumentNullException(nameof(container));

        if (directory == null)
            throw new ArgumentNullException(nameof(directory));

        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException($"'{nameof(newName)}' cannot be null or whitespace.", nameof(newName));
        }

        directory.ChangeParentId(parentId);
        directory.SetFileName(newName);
        directory.UpdateMimeType(_fileMimeTypeProvider.Get(newName));

        return directory;
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
