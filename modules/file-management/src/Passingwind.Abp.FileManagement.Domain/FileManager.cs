using Microsoft.Extensions.Options;
using Passingwind.Abp.FileManagement.Files;
using Passingwind.Abp.FileManagement.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement;

public class FileManager : DomainService, IFileManager
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
    protected IFileRepository FileRepository { get; }
    protected IFileUniqueIdGenerator FileUniqueIdGenerator { get; }

    public FileManager(
        IOptions<FileManagementOptions> fileManagementOptions,
        IFileRepository fileRepository,
        IFileContainerRepository fileContainerRepository,
        IBlobContainerFactory blobContainerFactor,
        IFileBlobNameGenerator fileBlobNameGenerator,
        IFileHashCalculator fileHashCalculator,
        IFileMimeTypeProvider fileMimeTypeProvider,
        IFileUniqueIdGenerator fileUniqueIdGenerator,
        IFileBlobContainerProvider fileBlobContainerProvider,
        IFileInfoCheckProvider fileInfoCheckProvider,
        IFileRenameProvider fileRenameProvider,
        IFileAccessTokenRepository fileAccessTokenRepository)
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
    }

    public virtual async Task<FileItem> ChangeFileNameAsync(Guid containerId, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or empty.", nameof(newFileName));
        }

        if (fileName == newFileName)
        {
            throw new ArgumentException("The new file name can't be same as origin name");
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        return await ChangeFileNameAsync(fileContainer, fileName, newFileName, parentId, cancellationToken);
    }

    public virtual async Task<FileItem> ChangeFileNameAsync(string container, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default)
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

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await ChangeFileNameAsync(fileContainer, fileName, newFileName, parentId, cancellationToken);
    }

    public virtual async Task CheckAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(Path.GetExtension(fileName));
        }

        await FileInfoCheckProvider.CheckAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task CheckAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(Path.GetExtension(fileName));
        }

        await FileInfoCheckProvider.CheckAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task ClearContainerFilesAsync(Guid containerId, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        await ClearContainerFilesAsync(fileContainer, cancellationToken);
    }

    public virtual async Task ClearContainerFilesAsync(string container, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        await ClearContainerFilesAsync(fileContainer, cancellationToken);
    }

    public virtual Task CopyDirectoryAsync(Guid containerId, Guid fileId, string targetFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task CopyDirectoryAsync(string container, Guid fileId, string targetFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task CopyFileAsync(Guid containerId, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);
        var stream = await GetFileSteamAsync(containerId, fileId, cancellationToken: cancellationToken) ?? throw new UserFriendlyException("The source file blob is not found.");
        await SaveAsync(containerId, fileName: file.FileName, stream: stream, parentId: targetParentId, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    public virtual async Task CopyFileAsync(string container, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(targetFileName))
        {
            throw new ArgumentException($"'{nameof(targetFileName)}' cannot be null or empty.", nameof(targetFileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        await CopyFileAsync(containerId: fileContainer.Id, fileId: fileId, targetFileName: targetFileName, targetParentId: targetParentId, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileAccessToken> CreateAccessTokenAsync(Guid containerId, Guid fileId, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var token = await GenerateTokenAsync(containerId, fileId, cancellationToken);

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        if (file?.IsDirectory != false)
        {
            throw new EntityNotFoundException(typeof(FileItem));
        }

        DateTime? expirationTime = expiration.HasValue ? Clock.Now.Add(expiration.Value) : null;
        var entity = new FileAccessToken(GuidGenerator.Create(), containerId, fileId, file.FileName, file.Length, file.MimeType, token, expirationTime);

        // add record to DB
        return await FileAccessTokenRepository.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> CreateArchiveAsync(Guid containerId, Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        if (!overrideExisting && await FileRepository.IsFileNameExistsAsync(containerId, archiveFileName, parentId: targetParentId, isDirectory: false, cancellationToken: cancellationToken))
        {
            throw new FileItemAlreadyExistsException();
        }

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var result = new MemoryStream();
        using var zip = new ZipArchive(result, ZipArchiveMode.Create, false);

        if (file.IsDirectory)
        {
            var files = await GetFilesAsync(containerId, fileId, includeSubDirectory, cancellationToken);
            foreach (var item in files)
            {
                // TODO: directory name
                var blobStream = await GetFileSteamAsync(containerId, item.Id, cancellationToken);
                if (blobStream == null)
                {
                    throw new BlobNotFoundException($"The file id {item.Id} blob not found.");
                }
                var entry = zip.CreateEntry(item.FileName);
                await blobStream.CopyToAsync(entry.Open());
            }
        }
        else
        {
            var blobStream = await GetFileSteamAsync(containerId, file.Id, cancellationToken);
            if (blobStream == null)
            {
                throw new BlobNotFoundException($"The file id {file.Id} blob not found.");
            }
            var entry = zip.CreateEntry(file.FileName);
            await blobStream.CopyToAsync(entry.Open());
        }

        //
        return await SaveAsync(containerId, archiveFileName, result, parentId: targetParentId, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> CreateArchiveAsync(string container, Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(container))
        {
            throw new ArgumentException($"'{nameof(container)}' cannot be null or empty.", nameof(container));
        }

        if (string.IsNullOrEmpty(archiveFileName))
        {
            throw new ArgumentException($"'{nameof(archiveFileName)}' cannot be null or empty.", nameof(archiveFileName));
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await CreateArchiveAsync(fileContainer.Id, fileId, archiveFileName, includeSubDirectory, targetParentId, overrideExisting, cancellationToken);
    }

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

        return await CreateDirectoryAsync(fileContainer, fileName, parentId, cancellationToken);
    }

    public virtual async Task<FileItem> CreateDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        return await CreateDirectoryAsync(fileContainer, fileName, parentId, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(string container, string fileName, Guid? parentId = null, bool forceDelete = false, CancellationToken cancellationToken = default)
    {
        var file = await FindAsync(container, fileName, parentId, cancellationToken);

        return file != null && await DeleteAsync(container, file.Id, forceDelete, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(string container, Guid fileId, bool forceDelete = false, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await DeleteAsync(fileContainer.Id, fileId, forceDelete, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid containerId, Guid fileId, bool forceDelete = false, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);
        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken: cancellationToken);

        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        if (file == null)
        {
            return false;
        }

        if (file.IsDirectory && forceDelete)
        {
            // TODO
        }

        if (fileContainer.AutoDeleteBlob && !file.IsDirectory)
        {
            await blobContainer.DeleteAsync(file.BlobName, cancellationToken);
        }

        // record
        await FileRepository.DeleteAsync(file, cancellationToken: cancellationToken);

        return true;
    }

    public virtual async Task<FileAccessToken?> FindAccessTokenAsync(Guid tokenId, CancellationToken cancellationToken = default)
    {
        return await FileAccessTokenRepository.FindAsync(tokenId, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem?> FindAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return await FileRepository.FindAsync(fileId, cancellationToken: cancellationToken);
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

    public virtual Task<string> GenerateTokenAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.NewGuid().ToString("N"));
    }

    public virtual async Task<FileItem> GetAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        return await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);
    }

    public async Task<FileItem> GetAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return await GetAsync(fileContainer.Id, fileName, parentId, cancellationToken);
    }

    public async Task<FileItem> GetAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return await FileRepository.GetByNameAsync(containerId, fileName, parentId, cancellationToken: cancellationToken);
    }

    public Task<FileItem> GetDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileItem> GetDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileItem> GetFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<FileItem> GetFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(string container, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(container, fileName, parentId, cancellationToken);

        if (file == null)
        {
            return null;
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetAllBytesOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<long> GetFileCountAsync(string container, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        return fileContainer.FilesCount;
    }

    public virtual async Task<Stream?> GetFileSteamAsync(string container, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await FileRepository.GetAsync(fileId, cancellationToken: cancellationToken);

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileSteamAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var file = await FindFileAsync(container, fileName, parentId, cancellationToken);

        if (file == null)
        {
            return null;
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        return await blobContainer.GetOrNullAsync(file.BlobName, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<string>> GetTagsAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(fileId, cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : (IReadOnlyList<string>)entity.Tags;
    }

    public virtual async Task<IReadOnlyList<string>> GetTagsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(container, fileName, parentId, cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : entity.Tags.ConvertAll(x => x.Tags);
    }

    public virtual async Task<IReadOnlyList<string>> GetTagsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(containerId, fileName, parentId, cancellationToken);

        return entity == null ? throw new EntityNotFoundException(typeof(FileItem)) : entity.Tags.ConvertAll(x => x.Tags);
    }

    public Task<IReadOnlyList<string>> GetTagsAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

        return await FileRepository.IsFileNameExistsAsync(fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: true, cancellationToken: cancellationToken);
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

        return await FileRepository.IsFileNameExistsAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        parentId ??= Guid.Empty;

        return await FileRepository.IsFileNameExistsAsync(containerId: fileContainer.Id, fileName: fileName, parentId: parentId, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsFileExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
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

        return await FileRepository.IsFileNameExistsAsync(fileContainer.Id, fileName: fileName, parentId: parentId, isDirectory: false, cancellationToken: cancellationToken);
    }
    public virtual Task<bool> IsReadOnlyAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<bool> IsReadOnlyAsync(Guid fileId, bool isReadOnly, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsReadOnlyAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsReadOnlyAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<bool> IsValidAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(Path.GetExtension(fileName));
        }

        return await FileInfoCheckProvider.IsValidAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> IsValidAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default)
    {
        var fileContainer = await FileContainerRepository.GetAsync(containerId, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(mimeType))
        {
            mimeType = FileMimeTypeProvider.Get(Path.GetExtension(fileName));
        }

        return await FileInfoCheckProvider.IsValidAsync(fileContainer, fileName, mimeType!, length, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(string container, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
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

        return await SaveAsync(container: container, fileName: fileName, bytes: ms.ToArray(), mimeType: mimeType, parentId: parentId, ignoreCheck: ignoreCheck, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(string container, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
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

        return await SaveAsync(containerId: fileContainer.Id, fileName: fileName, bytes: bytes, mimeType: mimeType, parentId: parentId, ignoreCheck: ignoreCheck, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(Guid containerId, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
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

        return await SaveAsync(containerId: containerId, fileName: fileName, bytes: ms.ToArray(), mimeType: mimeType, parentId: parentId, ignoreCheck: ignoreCheck, overrideExisting: overrideExisting, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> SaveAsync(Guid containerId, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
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

        var blobContainer = await FileBlobContainerProvider.GetAsync(fileContainer, cancellationToken);

        if (parentId == null)
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
            entity = await FileRepository.GetByNameAsync(fileContainer.Id, fileName, parentId, false, cancellationToken);
            entity.SetLength(length);
            entity.SetHash(hash);
        }
        else
        {
            var fileId = GuidGenerator.Create();
            var uniqueId = await FileUniqueIdGenerator.CreateAsync(fileContainer.Id, fileId, fileName, true, cancellationToken);
            var blobName = await FileBlobNameGenerator.CreateAsync(fileContainer.Id, fileId, uniqueId, fileName, cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(mimeType))
            {
                mimeType = FileMimeTypeProvider.Get(Path.GetExtension(fileName));
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

            await FileRepository.InsertAsync(entity, cancellationToken: cancellationToken);
        }

        // save blob
        await blobContainer.SaveAsync(entity.BlobName, bytes, true, cancellationToken: cancellationToken);

        return entity;
    }

    public virtual Task SetReadOnlyAsync(Guid fileId, bool isReadOnly, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetReadOnlyAsync(Guid containerId, Guid fileId, bool isReadOnly, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task SetTagsAsync(Guid fileId, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var entity = await FindAsync(fileId, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(FileItem));
        }

        entity.AddTags(tags.ToArray());
    }

    public virtual async Task SetTagsAsync(Guid containerId, Guid fileId, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        if (tags == null)
        {
            throw new ArgumentNullException(nameof(tags));
        }

        var entity = await FindAsync(fileId, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(FileItem));
        }

        entity.AddTags(tags.ToArray());
    }

    public virtual Task UnarchiveAsync(Guid containerId, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task UnarchiveAsync(string container, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected virtual async Task<FileItem> ChangeFileNameAsync(FileContainer container, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (container is null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (string.IsNullOrEmpty(newFileName))
        {
            throw new ArgumentException($"'{nameof(newFileName)}' cannot be null or empty.", nameof(newFileName));
        }

        if (fileName == newFileName)
        {
            throw new ArgumentException("The new file name can't be same as origin name");
        }

        var file = await FindAsync(container.Id, fileName, parentId, cancellationToken);

        if (file == null)
        {
            throw new EntityNotFoundException(typeof(FileItem));
        }

        var name = await FileRenameProvider.RenameAsync(container, newFileName, parentId, cancellationToken: cancellationToken);

        file.SetFileName(name);

        await FileRepository.UpdateAsync(file, cancellationToken: cancellationToken);

        return file;
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

    protected async Task<FileItem> CreateDirectoryAsync(FileContainer container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        var fileId = GuidGenerator.Create();
        var uniqueId = await FileUniqueIdGenerator.CreateAsync(container.Id, fileId, fileName, true, cancellationToken);
        var blobName = await FileBlobNameGenerator.CreateAsync(container.Id, fileId, uniqueId, fileName, cancellationToken: cancellationToken);

        var entity = new FileItem(
            fileId,
            container.Id,
            isDirectory: true,
            fileName: fileName,
            blobName: blobName,
            parentId: parentId ?? Guid.Empty,
            uniqueId: uniqueId,
            tenantId: CurrentTenant.Id);

        return await FileRepository.InsertAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task<List<FileItem>> GetFilesAsync(string container, string directoryName, Guid? parentId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
    {
        if (parentId == null)
        {
            parentId = Guid.Empty;
        }

        var fileContainer = await FileContainerRepository.GetByNameAsync(container, cancellationToken);

        var directory = await FileRepository.GetAsync(x => x.FileName == directoryName && x.IsDirectory && x.ParentId == parentId, cancellationToken: cancellationToken);

        return await GetFilesAsync(fileContainer.Id, directory.Id, includeSubDirectory, cancellationToken);
    }

    public async Task<List<FileItem>> GetFilesAsync(Guid containerId, string directoryName, Guid? parentId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
    {
        if (parentId == null)
        {
            parentId = Guid.Empty;
        }

        var directory = await FileRepository.GetAsync(x => x.FileName == directoryName && x.IsDirectory && x.ParentId == parentId, cancellationToken: cancellationToken);

        return await GetFilesAsync(containerId, directory.Id, includeSubDirectory, cancellationToken);
    }

    public async Task<List<FileItem>> GetFilesAsync(Guid containerId, Guid directoryId, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
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
}
