using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Options;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Passingwind.Abp.FileManagement;

[Obsolete]
public class FileManager : FileItemManager, IFileManager, IScopedDependency
{
    public FileManager(IOptions<FileManagementOptions> fileManagementOptions, IFileItemRepository fileRepository, IFileContainerRepository fileContainerRepository, IBlobContainerFactory blobContainerFactor, IFileBlobNameGenerator fileBlobNameGenerator, IFileHashCalculator fileHashCalculator, IFileMimeTypeProvider fileMimeTypeProvider, IFileUniqueIdGenerator fileUniqueIdGenerator, IFileBlobContainerProvider fileBlobContainerProvider, IFileInfoCheckProvider fileInfoCheckProvider, IFileRenameProvider fileRenameProvider, IFileAccessTokenRepository fileAccessTokenRepository, IUnitOfWorkManager unitOfWorkManager) : base(fileManagementOptions, fileRepository, fileContainerRepository, blobContainerFactor, fileBlobNameGenerator, fileHashCalculator, fileMimeTypeProvider, fileUniqueIdGenerator, fileBlobContainerProvider, fileInfoCheckProvider, fileRenameProvider, fileAccessTokenRepository, unitOfWorkManager)
    {
    }
}

/// <inheritdoc/>
public class FileItemManager : DomainService, IFileItemManager
{
    protected IBlobContainerFactory BlobContainerFactor { get; }
    protected IFileAccessTokenRepository FileAccessTokenRepository { get; }
    protected IFileBlobContainerProvider FileBlobContainerProvider { get; }
    protected IFileBlobNameGenerator FileBlobNameGenerator { get; }
    protected IFileContainerRepository FileContainerRepository { get; }
    protected IFileHashCalculator FileHashCalculator { get; }
    protected IFileInfoCheckProvider FileInfoCheckProvider { get; }
    protected FileManagementOptions FileManagementOptions { get; }
    protected IFileMimeTypeProvider FileMimeTypeProvider { get; }
    protected IFileRenameProvider FileRenameProvider { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileUniqueIdGenerator FileUniqueIdGenerator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public FileItemManager(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileItemRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IBlobContainerFactory blobContainerFactor,
        IFileBlobNameGenerator fileBlobNameGenerator,
        IFileHashCalculator fileHashCalculator,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileUniqueIdGenerator fileUniqueIdGenerator,
        IFileBlobContainerProvider fileBlobContainerProvider,
        IFileInfoCheckProvider fileInfoCheckProvider,
        IFileRenameProvider fileRenameProvider,
        IFileAccessTokenRepository fileAccessTokenRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        FileManagementOptions = fileManagementOptions.Value;
        FileRepository = fileRepository;
        FileContainerRepository = fileContainerRepository;
        BlobContainerFactor = blobContainerFactor;
        FileBlobNameGenerator = fileBlobNameGenerator;
        FileHashCalculator = fileHashCalculator;
        FileMimeTypeProvider = fileMimeTypeProvider;
        FileUniqueIdGenerator = fileUniqueIdGenerator;
        FileBlobContainerProvider = fileBlobContainerProvider;
        FileInfoCheckProvider = fileInfoCheckProvider;
        FileRenameProvider = fileRenameProvider;
        FileAccessTokenRepository = fileAccessTokenRepository;
        UnitOfWorkManager = unitOfWorkManager;
    }

    #region Find & Get

    public virtual async Task<FileItem?> FindAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return await FileRepository.FindAsync(fileId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.FindByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await GetAsync(fileContainer.Id, fileName, parentId, cancellationToken);
    }

    public virtual async Task<FileItem> GetAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return await FileRepository.GetByNameAsync(containerId, fileName, parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.GetByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.GetByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.GetByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.GetByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }

    #endregion Find & Get

    #region List

    public virtual async Task<List<FileItem>> GetFilesAsync(string containerName, Guid? directoryId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
    {
        var container = await FileContainerRepository.GetByNameAsync(containerName, cancellationToken);

        return await GetFilesAsync(container.Id, directoryId, includeSubDirectory, cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetFilesAsync(Guid containerId, Guid? directoryId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
    {
        var list = await FileRepository.GetListAsync(containerId: containerId, parentId: directoryId, cancellationToken: cancellationToken);

        if (includeSubDirectory)
        {
            foreach (var item in list)
            {
                list.AddRange(await GetFilesAsync(containerId: containerId, directoryId: item.Id, includeSubDirectory: true, cancellationToken: cancellationToken));
            }
        }

        return list;
    }

    #endregion List

    #region Exists

    public virtual async Task<bool> IsDirectoryExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await IsDirectoryExistsAsync(fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsDirectoryExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        _ = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.IsFileNameExistsAsync(containerId, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await IsExistsAsync(fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.IsFileNameExistsAsync(fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsFileExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        parentId ??= Guid.Empty;

        return await IsFileExistsAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsFileExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.IsFileNameExistsAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    #endregion Exists

    #region Get file bytes & stream

    public virtual async Task<byte[]?> GetFileBytesAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await GetFileBytesAsync(fileContainer.Id, fileName, parentId, cancellationToken);
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
        }

        var file = await FileRepository.FindByNameAsync(containerId, fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);

        if (file == null)
            throw new EntityNotFoundException();

        return await GetFileBytesAsync(file.Id, cancellationToken);
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetAsync(file.ContainerId, cancellationToken: cancellationToken);
        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await GetFileSteamAsync(fileContainer.Id, fileName, parentId, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
        }

        var file = await FileRepository.FindByNameAsync(containerId, fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);

        if (file == null)
            throw new EntityNotFoundException();

        return await GetFileSteamAsync(file.Id, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetAsync(file.ContainerId, cancellationToken: cancellationToken);
        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    #endregion Get file bytes & stream

    #region Create directory

    public virtual async Task<FileItem> CreateDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await CreateDirectoryAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> CreateDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        return await CreateDirectoryAsync(container: fileContainer, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    protected virtual async Task<FileItem> CreateDirectoryAsync(FileContainer container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (!await IsFileNameValidAsync(fileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileNameInvalid);
        }

        var fileId = GuidGenerator.Create();
        var uniqueId = await FileUniqueIdGenerator.CreateAsync(container.Id, fileId, fileName, true, cancellationToken);
        var blobName = await FileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, fileName, cancellationToken: cancellationToken);

        string dirPath = "/";
        if (parentId.HasValue && parentId.Value != Guid.Empty)
        {
            var parent = await FileRepository.GetAsync(parentId.Value, cancellationToken: cancellationToken);
            dirPath = parent.Path.FullPath;
        }
        else
        {
            parentId = Guid.Empty;
        }

        var entity = new FileItem(
            fileId,
            container.Id,
            isDirectory: true,
            fileName: fileName,
            blobName: blobName,
            parentId: parentId ?? Guid.Empty,
            uniqueId: uniqueId,
            tenantId: CurrentTenant.Id);

        entity.SetFullPath(dirPath + fileName + "/");

        return await FileRepository.InsertAsync(entity, true, cancellationToken: cancellationToken);
    }

    #endregion Create directory

    #region Save

    public virtual async Task<FileItem> SaveAsync(string container, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, Dictionary<string, string?>? tags = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        stream.Dispose();

        return await SaveAsync(
            container: container,
            fileName: fileName,
            bytes: ms.ToArray(),
            mimeType: mimeType,
            parentId: parentId,
            ignoreCheck: ignoreCheck,
            overrideExisting: overrideExisting,
            tags: tags,
            cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(string container, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, Dictionary<string, string?>? tags = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await SaveAsync(
            fileContainer: fileContainer,
            fileName: fileName,
            bytes: bytes,
            mimeType: mimeType,
            parentId: parentId,
            ignoreCheck: ignoreCheck,
            overrideExisting: overrideExisting,
            tags: tags,
            cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(Guid containerId, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, Dictionary<string, string?>? tags = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        stream.Dispose();

        return await SaveAsync(
            containerId: containerId,
            fileName: fileName,
            bytes: ms.ToArray(),
            mimeType: mimeType,
            parentId: parentId,
            ignoreCheck: ignoreCheck,
            overrideExisting: overrideExisting,
            tags: tags,
            cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(Guid containerId, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, Dictionary<string, string?>? tags = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        return await SaveAsync(
            fileContainer,
            fileName: fileName,
            bytes: bytes,
            mimeType: mimeType,
            parentId: parentId,
            ignoreCheck: ignoreCheck,
            overrideExisting: overrideExisting,
            tags: tags,
            cancellationToken: cancellationToken);
    }

    protected virtual async Task<FileItem> SaveAsync(FileContainer fileContainer, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, Dictionary<string, string?>? tags = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (bytes is null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        if (!await IsFileNameValidAsync(fileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileNameInvalid);
        }

        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, true, isolationLevel: IsolationLevel.ReadCommitted))
        {
            var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

            string dirPath = "/";
            if (parentId.HasValue && parentId.Value != Guid.Empty)
            {
                var parent = await FileRepository.GetAsync(parentId.Value, cancellationToken: cancellationToken);
                dirPath = parent.Path.FullPath;
            }
            else
            {
                parentId = Guid.Empty;
            }

            var fileExists = await FileRepository.IsFileNameExistsAsync(
                containerId: fileContainer.Id,
                fileName: fileName,
                parentId: parentId,
                isDirectory: false,
                cancellationToken: cancellationToken);

            if (fileContainer.OverrideBehavior == FileOverrideBehavior.Override)
            {
                overrideExisting = true;
            }
            else if (fileContainer.OverrideBehavior == FileOverrideBehavior.Rename)
            {
                fileName = await FileRenameProvider.RenameAsync(fileContainer, fileName, parentId, isDirectory: false, cancellationToken: cancellationToken);
                fileExists = false;
            }

            if (!overrideExisting && fileExists && fileContainer.OverrideBehavior == FileOverrideBehavior.None)
            {
                throw new BusinessException(FileManagementErrorCodes.FileExists);
            }

            var length = bytes.Length < 1024 ? 1 : bytes.Length / 1024; // KB
            var hash = await FileHashCalculator.GetAsync(bytes, cancellationToken);

            FileItem entity;
            if (fileExists)
            {
                entity = await FileRepository.GetByNameAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
                entity.SetLength(length)
                    .SetHash(hash);

                if (tags != null)
                    entity.AddTags(tags);

                await FileRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);
            }
            else
            {
                var fileId = GuidGenerator.Create();
                var uniqueId = await FileUniqueIdGenerator.CreateAsync(fileContainer.Id, fileId, fileName, true, cancellationToken);
                var blobName = await FileBlobNameGenerator.CreateAsync(fileContainer.Id, fileId, uniqueId, fileName, cancellationToken: cancellationToken);

                if (string.IsNullOrWhiteSpace(mimeType))
                {
                    mimeType = FileMimeTypeProvider.Get(fileName);
                }

                if (!ignoreCheck)
                {
                    await FileInfoCheckProvider.CheckAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
                }

                entity = new FileItem(
                      fileId,
                      fileContainer.Id,
                      isDirectory: false,
                      fileName: fileName,
                      blobName: blobName,
                      parentId: parentId ?? Guid.Empty,
                      mimeType: mimeType,
                      length: length,
                      hash: hash,
                      uniqueId: uniqueId,
                      tenantId: CurrentTenant.Id);

                if (tags != null)
                    entity.AddTags(tags);

                entity.SetFullPath(dirPath + fileName);

                await FileRepository.InsertAsync(entity, true, cancellationToken: cancellationToken);
            }

            // save blob
            await blobContainer.SaveAsync(entity.BlobName, bytes, true, cancellationToken: cancellationToken);

            await uow.CompleteAsync();

            return entity;
        }
    }

    #endregion Save

    #region Delete

    public virtual async Task<bool> DeleteAsync(string container, string fileName, Guid? parentId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default)
    {
        var file = await FindAsync(container, fileName, parentId, cancellationToken);

        return file != null && await DeleteAsync(file.Id, forceDeleteBlob, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid containerId, string fileName, Guid? parentId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default)
    {
        var file = await FindAsync(containerId, fileName, parentId, cancellationToken);

        return file != null && await DeleteAsync(file.Id, forceDeleteBlob, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid fileId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.FindAsync(fileId, cancellationToken: cancellationToken);

        if (file == null)
        {
            return false;
        }

        var fileContainer = await FileContainerRepository.GetAsync(file.ContainerId, cancellationToken: cancellationToken);
        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken: cancellationToken);

        if (file.IsDirectory)
        {
            var subFiles = await GetFilesAsync(file.ContainerId, file.Id, false, cancellationToken);
            foreach (var item in subFiles)
            {
                await DeleteAsync(item.Id, forceDeleteBlob, cancellationToken);
            }
        }

        if (!file.IsDirectory && (fileContainer.AutoDeleteBlob || forceDeleteBlob))
        {
            await blobContainer.DeleteAsync(file.BlobName, cancellationToken);
        }

        // record
        await FileRepository.DeleteAsync(file, cancellationToken: cancellationToken);

        return true;
    }

    #endregion Delete

    #region Rename

    public virtual async Task<FileItem> ChangeNameAsync(string container, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or empty.", nameof(newFileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken: cancellationToken);

        return await ChangeNameAsync(fileContainer.Id, fileName, newFileName, parentId, cancellationToken);
    }

    public virtual async Task<FileItem> ChangeNameAsync(Guid containerId, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or empty.", nameof(newFileName));
        }

        var file = await GetAsync(containerId, fileName, parentId, cancellationToken);

        return await ChangeNameAsync(file, newFileName, cancellationToken);
    }

    public virtual async Task<FileItem> ChangeNameAsync(Guid fileId, string newFileName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or whitespace.", nameof(newFileName));
        }

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        return await ChangeNameAsync(file, newFileName, cancellationToken);
    }

    protected virtual async Task<FileItem> ChangeNameAsync(FileItem file, string newFileName, CancellationToken cancellationToken = default)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (string.IsNullOrEmpty(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or empty.", nameof(newFileName));
        }

        if (!await IsFileNameValidAsync(newFileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileNameInvalid);
        }

        var fileContainer = await FileContainerRepository.GetAsync(file.ContainerId, cancellationToken: cancellationToken);

        var name = await FileRenameProvider.RenameAsync(fileContainer, newFileName, file.ParentId, cancellationToken: cancellationToken);

        if (await IsExistsAsync(file.ContainerId, newFileName, file.ParentId, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists);
        }

        file.SetFileName(name);

        await FileRepository.UpdateAsync(file, true, cancellationToken: cancellationToken);

        await UnitOfWorkManager.Current!.SaveChangesAsync();

        await RefreshFullPathAsync(file.Id, cancellationToken);

        await UnitOfWorkManager.Current!.SaveChangesAsync();

        return file;
    }

    #endregion Rename

    #region FullPath

    //protected virtual void UpdateExistsFileFullPath(FileItem file)
    //{
    //    var fullPath = file.Path.FullPath;
    //    fullPath = fullPath.Substring(0, fullPath.LastIndexOf("/", 1)) + file.FileName;
    //    if (file.IsDirectory)
    //        fullPath += "/";
    //    file.SetFullPath(fullPath);
    //}

    private async Task UpdateParentPath(List<string> paths, Guid parentId)
    {
        if (parentId == Guid.Empty)
            return;

        var item = await FileRepository.GetAsync(parentId);

        paths.Add(item.FileName);

        await UpdateParentPath(paths, item.ParentId);
    }

    public virtual async Task RefreshFullPathAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        await RefreshFullPathAsync(file);

        await FileRepository.UpdateAsync(file, true, cancellationToken: cancellationToken);
    }

    protected virtual async Task RefreshFullPathAsync(FileItem file)
    {
        var paths = new List<string>();

        var fullPath = "/" + file.FileName;

        if (file.IsDirectory)
            fullPath += "/";

        await UpdateParentPath(paths, file.ParentId);
        paths.Reverse();

        if (paths.Count > 0)
        {
            fullPath = "/" + string.Join("/", paths) + fullPath;
        }

        file.SetFullPath(fullPath);
    }

    #endregion FullPath

    #region Clone

    public Task CloneFileAsync(Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CloneFileAsync(string container, string fileName, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CloneFileAsync(Guid containerId, string fileName, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion Clone

    #region Container

    public virtual async Task ClearContainerFilesAsync(string container, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        await ClearContainerFilesAsync(fileContainer, cancellationToken);
    }

    public virtual async Task ClearContainerFilesAsync(Guid containerId, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        await ClearContainerFilesAsync(fileContainer, cancellationToken);
    }

    protected virtual async Task ClearContainerFilesAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken: cancellationToken);

        // blob
        if (fileContainer.AutoDeleteBlob)
        {
            var files = await FileRepository.GetListAsync(containerId: fileContainer.Id, cancellationToken: cancellationToken);
            foreach (var file in files)
            {
                if (!file.IsDirectory)
                {
                    await blobContainer.DeleteAsync(file.BlobName, cancellationToken);
                }
            }
        }

        // record
        await FileRepository.DeleteDirectAsync(x => x.ContainerId == fileContainer.Id, cancellationToken);
    }

    public virtual async Task<long> GetFileCountAsync(string container, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return fileContainer.FilesCount;
    }

    #endregion Container

    #region File meta valid

    public virtual async Task<bool> IsValidAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(fileName);
        }

        return await FileInfoCheckProvider.IsValidAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsValidAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(fileName);
        }

        return await FileInfoCheckProvider.IsValidAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task CheckAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(fileName);
        }

        await FileInfoCheckProvider.CheckAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task CheckAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(fileName);
        }

        await FileInfoCheckProvider.CheckAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    #endregion File meta valid

    #region Tags

    public virtual async Task<Dictionary<string, string?>> GetTagsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var entity = await FindAsync(container, fileName, parentId, cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : entity.Tags.ToDictionary(x => x.Name, x => x.Value, StringComparer.InvariantCultureIgnoreCase);
    }

    public virtual async Task<Dictionary<string, string?>> GetTagsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var entity = await FindAsync(containerId, fileName, parentId, cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : entity.Tags.ToDictionary(x => x.Name, x => x.Value, StringComparer.InvariantCultureIgnoreCase);
    }

    public virtual async Task<Dictionary<string, string?>> GetTagsAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : entity.Tags.ToDictionary(x => x.Name, x => x.Value, StringComparer.InvariantCultureIgnoreCase);
    }

    public virtual async Task<string?> GetTagAsync(Guid fileId, string name, CancellationToken cancellationToken = default)
    {
        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);
        return entity.Tags.Find(x => x.Name == name)?.Value;
    }

    public virtual async Task<FileItem> AddTagsAsync(Guid fileId, Dictionary<string, string?> tags, CancellationToken cancellationToken = default)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        entity.AddTags(tags);

        return await FileRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> AddTagAsync(Guid fileId, string name, string? value = null, CancellationToken cancellationToken = default)
    {
        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        entity.AddTag(name, value);

        return await FileRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SetTagsAsync(Guid fileId, Dictionary<string, string?> tags, CancellationToken cancellationToken = default)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        entity.Tags.Clear();
        entity.AddTags(tags);

        return await FileRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);
    }

    public virtual async Task RemoveTagsAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var entity = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        entity.Tags.Clear();

        await FileRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);
    }

    #endregion Tags

    #region Share

    public Task<string> GenerateAccessTokenAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"));
    }

    public virtual async Task<FileAccessToken> CreateAccessTokenAsync(Guid fileId, DateTime? expiration = null, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        if (file?.IsDirectory != false)
        {
            throw new EntityNotFoundException(typeof(FileItem));
        }

        var token = await GenerateAccessTokenAsync(fileId, cancellationToken);

        var entity = new FileAccessToken(GuidGenerator.Create(), file.ContainerId, fileId, file.FileName, file.Length, file.MimeType, token, expiration);

        // add record to DB
        return await FileAccessTokenRepository.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileAccessToken?> FindAccessTokenAsync(Guid tokenId, CancellationToken cancellationToken = default)
    {
        return await FileAccessTokenRepository.FindAsync(tokenId, cancellationToken: cancellationToken);
    }

    #endregion Share

    #region Archive

    public virtual async Task<FileItem> CreateArchiveAsync(string container, string fileName, Guid? parentId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        var file = await GetFileAsync(container, fileName, parentId, cancellationToken);

        return await CreateArchiveAsync(file, archiveFileName, includeSubDirectory, targetParentId, overrideExisting, cancellationToken);
    }

    public virtual async Task<FileItem> CreateArchiveAsync(Guid containerId, string fileName, Guid? parentId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        var file = await GetFileAsync(containerId, fileName, parentId, cancellationToken);

        return await CreateArchiveAsync(file, archiveFileName, includeSubDirectory, targetParentId, overrideExisting, cancellationToken);
    }

    public virtual async Task<FileItem> CreateArchiveAsync(Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        return await CreateArchiveAsync(file, archiveFileName, includeSubDirectory, targetParentId, overrideExisting, cancellationToken);
    }

    protected virtual async Task<FileItem> CreateArchiveAsync(FileItem file, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        var fileId = file.Id;
        var containerId = file.ContainerId;

        if (!overrideExisting && await FileRepository.IsFileNameExistsAsync(containerId, archiveFileName, parentId: targetParentId, isDirectory: false, cancellationToken: cancellationToken))
        {
            throw new FileItemAlreadyExistsException();
        }

        var result = new MemoryStream();
        using var zip = new ZipArchive(result, ZipArchiveMode.Create, false);

        if (file.IsDirectory)
        {
            if (includeSubDirectory)
                await LoopAddArchiveItemAsync(zip, containerId, file.Id, string.Empty, cancellationToken);
            else
                throw new InvalidOperationException("Can't create archive for an directory.");
        }
        else
        {
            var blobStream = await GetFileSteamAsync(fileId, cancellationToken) ?? throw new BlobNotFoundException($"The file id {file.Id} blob not found.");

            var entry = zip.CreateEntry(file.FileName);
            await blobStream.CopyToAsync(entry.Open());
        }

        //
        return await SaveAsync(containerId, archiveFileName, result, parentId: targetParentId, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    protected virtual async Task LoopAddArchiveItemAsync(ZipArchive archive, Guid containerId, Guid fileId, string archivePath, CancellationToken cancellationToken = default)
    {
        var item = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        if (item.IsDirectory)
        {
            var files = await FileRepository.GetListAsync(containerId: containerId, parentId: item.Id, cancellationToken: cancellationToken);

            foreach (var subFile in files)
            {
                await LoopAddArchiveItemAsync(archive, containerId, subFile.Id, Path.Combine(archivePath, item.FileName), cancellationToken);
            }
        }
        else
        {
            var blobStream = await GetFileSteamAsync(fileId, cancellationToken) ?? throw new BlobNotFoundException($"The file id {fileId} blob not found.");

            var entry = archive.CreateEntry(Path.Combine(archivePath, item.FileName));
            await blobStream.CopyToAsync(entry.Open());
        }
    }

    #endregion Archive

    #region Move

    public virtual async Task MoveAsync(string container, string fileName, Guid? parentId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(targetFileName))
        {
            throw new ArgumentException($"'{nameof(targetFileName)}' cannot be null or empty.", nameof(targetFileName));
        }

        var file = await GetFileAsync(container, fileName, parentId, cancellationToken);

        await MoveAsync(file, targetFileName, targetParentId, overrideExisting, cancellationToken);
    }

    public virtual async Task MoveAsync(Guid containerId, string fileName, Guid? parentId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(targetFileName))
        {
            throw new ArgumentException($"'{nameof(targetFileName)}' cannot be null or empty.", nameof(targetFileName));
        }

        var file = await GetFileAsync(containerId, fileName, parentId, cancellationToken);

        await MoveAsync(file, targetFileName, targetParentId, overrideExisting, cancellationToken);
    }

    public virtual async Task MoveAsync(Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(targetFileName))
        {
            throw new ArgumentException($"'{nameof(targetFileName)}' cannot be null or empty.", nameof(targetFileName));
        }

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        await MoveAsync(file, targetFileName, targetParentId, overrideExisting, cancellationToken);
    }

    protected virtual async Task MoveAsync(FileItem file, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (!await IsFileNameValidAsync(targetFileName, cancellationToken))
        {
            throw new BusinessException(FileManagementErrorCodes.FileNameInvalid);
        }

        var exists = await IsExistsAsync(file.ContainerId, targetFileName, targetParentId, cancellationToken);

        if (!overrideExisting && exists)
        {
            throw new BusinessException(FileManagementErrorCodes.FileExists).WithData("fileName", targetFileName);
        }

        if (exists)
        {
            var existsTarget = await GetAsync(file.ContainerId, targetFileName, targetParentId, cancellationToken);
            await DeleteAsync(existsTarget.Id, cancellationToken: cancellationToken);
        }

        file.SetFileName(targetFileName)
            .ChangeParentId(targetParentId ?? Guid.Empty);

        await RefreshFullPathAsync(file);

        await FileRepository.UpdateAsync(file, true, cancellationToken: cancellationToken);
    }

    #endregion Move

    #region File name

    public virtual async Task<bool> IsFileNameValidAsync(string fileName, CancellationToken cancellationToken = default)
    {
        return await FileInfoCheckProvider.IsFileNameValidAsync(fileName, cancellationToken);
    }

    #endregion File name
}
