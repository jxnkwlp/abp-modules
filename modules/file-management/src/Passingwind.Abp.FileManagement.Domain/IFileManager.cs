using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement;

[Obsolete]
public interface IFileManager : IFileItemManager;

/// <summary>
///  The file (directory) manager service
/// </summary>
public interface IFileItemManager : IDomainService
{
    #region Get & Find

    /// <summary>
    ///  Find an file (directory) by ID
    /// </summary>
    Task<FileItem?> FindAsync(Guid fileId, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get an file (directory) by ID
    /// </summary>
    Task<FileItem> GetAsync(Guid fileId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Find an file (directory)
    /// </summary>
    Task<FileItem?> FindAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Find an file (directory)
    /// </summary>
    Task<FileItem?> FindAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Get an file (directory)
    /// </summary>
    Task<FileItem> GetAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get an file (directory)
    /// </summary>
    Task<FileItem> GetAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Find an file item
    /// </summary>
    Task<FileItem?> FindFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Find an file item
    /// </summary>
    Task<FileItem?> FindFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Get an file item
    /// </summary>
    Task<FileItem> GetFileAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get an file item
    /// </summary>
    Task<FileItem> GetFileAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Find an directory item
    /// </summary>
    Task<FileItem?> FindDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Find an directory item
    /// </summary>
    Task<FileItem?> FindDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Get an directory item
    /// </summary>
    Task<FileItem> GetDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Get an directory item
    /// </summary>
    Task<FileItem> GetDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    #endregion Get & Find

    #region List

    /// <summary>
    ///  List Files
    /// </summary>
    Task<List<FileItem>> GetFilesAsync(string containerName, Guid? directoryId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///  List Files
    /// </summary>
    Task<List<FileItem>> GetFilesAsync(Guid containerId, Guid? directoryId = null, bool includeSubDirectory = false, CancellationToken cancellationToken = default);

    #endregion List

    #region Exists

    Task<bool> IsExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<bool> IsFileExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsFileExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<bool> IsDirectoryExistsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<bool> IsDirectoryExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    #endregion Exists

    #region Get file bytes & stream

    Task<byte[]?> GetFileBytesAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<byte[]?> GetFileBytesAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<byte[]?> GetFileBytesAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task<Stream?> GetFileSteamAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamAsync(Guid fileId, CancellationToken cancellationToken = default);

    #endregion Get file bytes & stream

    #region Create directory

    /// <summary>
    ///  Create directory and save record
    /// </summary>
    Task<FileItem> CreateDirectoryAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Create directory and save record
    /// </summary>
    Task<FileItem> CreateDirectoryAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);

    #endregion Create directory

    #region Save

    /// <summary>
    ///  Save an file from stream and return the file record infomartion
    /// </summary>
    Task<FileItem> SaveAsync(string container, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Save an file from bytes and return the file record infomartion
    /// </summary>
    Task<FileItem> SaveAsync(string container, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Save an file from stream and return the file record infomartion
    /// </summary>
    Task<FileItem> SaveAsync(Guid containerId, string fileName, Stream stream, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Save an file from bytes and return the file record infomartion
    /// </summary>
    Task<FileItem> SaveAsync(Guid containerId, string fileName, byte[] bytes, string? mimeType = null, Guid? parentId = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);

    #endregion Save

    #region Delete

    Task<bool> DeleteAsync(string container, string fileName, Guid? parentId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid containerId, string fileName, Guid? parentId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid fileId, bool forceDeleteBlob = false, CancellationToken cancellationToken = default);

    #endregion Delete

    #region Rename

    Task<FileItem> ChangeNameAsync(string container, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> ChangeNameAsync(Guid containerId, string fileName, string newFileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<FileItem> ChangeNameAsync(Guid fileId, string newFileName, CancellationToken cancellationToken = default);

    #endregion Rename

    #region Clone

    /// <summary>
    ///  Not Implemented
    /// </summary>
    Task CloneFileAsync(Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Not Implemented
    /// </summary>
    Task CloneFileAsync(string container, string fileName, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Not Implemented
    /// </summary>
    Task CloneFileAsync(Guid containerId, string fileName, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    // Task CloneDirectoryAsync();

    #endregion Clone

    #region Container

    Task ClearContainerFilesAsync(string container, CancellationToken cancellationToken = default);
    Task ClearContainerFilesAsync(Guid containerId, CancellationToken cancellationToken = default);

    Task<long> GetFileCountAsync(string container, CancellationToken cancellationToken = default);

    #endregion Container

    #region File meta valid

    Task<bool> IsValidAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);
    Task<bool> IsValidAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);

    Task CheckAsync(string container, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);
    Task CheckAsync(Guid containerId, string fileName, long length, string? mimeType = null, CancellationToken cancellationToken = default);

    #endregion File meta valid

    #region ReadOnly

    //Task<bool> IsReadOnlyAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    //Task<bool> IsReadOnlyAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    //Task<bool> IsReadOnlyAsync(Guid fileId, CancellationToken cancellationToken = default);
    //Task SetReadOnlyAsync(Guid fileId, bool isReadOnly, CancellationToken cancellationToken = default);

    #endregion ReadOnly

    #region Tags

    Task<Dictionary<string, string?>> GetTagsAsync(string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string?>> GetTagsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default);
    Task<Dictionary<string, string?>> GetTagsAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task<string?> GetTagAsync(Guid fileId, string name, CancellationToken cancellationToken = default);

    Task<FileItem> SetTagsAsync(Guid fileId, Dictionary<string, string?> tags, CancellationToken cancellationToken = default);

    Task<FileItem> AddTagsAsync(Guid fileId, Dictionary<string, string?> tags, CancellationToken cancellationToken = default);

    Task<FileItem> AddTagAsync(Guid fileId, string name, string? value = null, CancellationToken cancellationToken = default);

    Task RemoveTagsAsync(Guid fileId, CancellationToken cancellationToken = default);

    #endregion Tags

    #region Share

    /// <summary>
    ///  Generate new access token
    /// </summary>
    Task<string> GenerateAccessTokenAsync(Guid fileId, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Create file access token
    /// </summary>
    Task<FileAccessToken> CreateAccessTokenAsync(Guid fileId, DateTime? expiration = null, CancellationToken cancellationToken = default);
    /// <summary>
    ///  get file access token by token ID
    /// </summary>
    Task<FileAccessToken?> FindAccessTokenAsync(Guid tokenId, CancellationToken cancellationToken = default);

    #endregion Share

    #region Archive

    /// <summary>
    ///  Create Archive
    /// </summary>
    Task<FileItem> CreateArchiveAsync(string container, string fileName, Guid? parentId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Create Archive
    /// </summary>
    Task<FileItem> CreateArchiveAsync(Guid containerId, string fileName, Guid? parentId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Create Archive
    /// </summary>
    Task<FileItem> CreateArchiveAsync(Guid fileId, string archiveFileName, bool includeSubDirectory = true, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    //Task UnarchiveAsync(string container, Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    //Task UnarchiveAsync(Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);

    #endregion Archive

    #region Move

    /// <summary>
    ///  Move an file(directory)
    /// </summary>
    Task MoveAsync(string container, string fileName, Guid? parentId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Move an file(directory)
    /// </summary>
    Task MoveAsync(Guid containerId, string fileName, Guid? parentId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    /// <summary>
    ///  Move an file(directory)
    /// </summary>
    Task MoveAsync(Guid fileId, string targetFileName, Guid? targetParentId = null, bool overrideExisting = false, CancellationToken cancellationToken = default);
    #endregion Move

    #region File Path

    Task RefreshFullPathAsync(Guid fileId, CancellationToken cancellationToken = default);

    #endregion File Path

    #region File name

    Task<bool> IsFileNameValidAsync(string fileName, CancellationToken cancellationToken = default);

    #endregion File name
}
