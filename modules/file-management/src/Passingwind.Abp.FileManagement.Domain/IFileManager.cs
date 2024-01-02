using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement;

public interface IFileManager : IDomainService
{
    Task<FileItem?> FindAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<FileItem> GetAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task<FileItem?> FindAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem?> FindAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> GetAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> GetAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem?> FindFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem?> FindFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> GetFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> GetFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem?> FindDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem?> FindDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> GetDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> GetDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<List<FileItem>> GetFilesAsync(string container, string directoryName, Guid? parentId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default);
    Task<List<FileItem>> GetFilesAsync(Guid containerId, string directoryName, Guid? parentId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default);
    Task<List<FileItem>> GetFilesAsync(Guid containerId, Guid directoryId, bool includeSubDirectory = false, CancellationToken cancellationToken = default);

    Task<bool> IsExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<bool> IsFileExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsFileExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<bool> IsDirectoryExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsDirectoryExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<byte[]?> GetFileBytesAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<byte[]?> GetFileBytesAsync(string container, Guid fileId, CancellationToken cancellationToken = default);
    Task<byte[]?> GetFileBytesAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default);

    Task<Stream?> GetFileSteamAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamAsync(string container, Guid fileId, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default);

    Task<FileItem> CreateDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> CreateDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> ChangeFileNameAsync(string container, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> ChangeFileNameAsync(Guid containerId, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<FileItem> SaveAsync(string container, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task<FileItem> SaveAsync(string container, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task<FileItem> SaveAsync(Guid containerId, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task<FileItem> SaveAsync(Guid containerId, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string container, string fileName, Guid? parentId = null, bool forceDelete = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string container, Guid fileId, bool forceDelete = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid containerId, Guid fileId, bool forceDelete = false, CancellationToken cancellationToken = default);

    Task ClearContainerFilesAsync(string container, CancellationToken cancellationToken = default);
    Task ClearContainerFilesAsync(Guid containerId, CancellationToken cancellationToken = default);

    Task<long> GetFileCountAsync(string container, CancellationToken cancellationToken = default);

    Task<bool> IsValidAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);
    Task<bool> IsValidAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);

    Task CheckAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);
    Task CheckAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);

    Task<bool> IsReadOnlyAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsReadOnlyAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsReadOnlyAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default);

    Task SetReadOnlyAsync(Guid containerId, Guid fileId, bool isReadOnly, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetTagsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetTagsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetTagsAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default);

    Task SetTagsAsync(Guid containerId, Guid fileId, IEnumerable<string> tags, CancellationToken cancellationToken = default);

    Task<string> GenerateTokenAsync(Guid containerId, Guid fileId, CancellationToken cancellationToken = default);
    Task<FileAccessToken> CreateAccessTokenAsync(Guid containerId, Guid fileId, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task<FileAccessToken?> FindAccessTokenAsync(Guid tokenId, CancellationToken cancellationToken = default);

    Task<FileItem> CreateArchiveAsync(string container, Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task<FileItem> CreateArchiveAsync(Guid containerId, Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    Task UnarchiveAsync(string container, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task UnarchiveAsync(Guid containerId, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    Task CopyFileAsync(Guid containerId, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task CopyFileAsync(string container, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    Task CopyDirectoryAsync(Guid containerId, Guid fileId, string targetFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task CopyDirectoryAsync(string container, Guid fileId, string targetFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
}
