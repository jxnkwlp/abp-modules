using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public interface IFileFluentManager
{
    Task<FileFluentItem?> FindAsync(string fullPath, CancellationToken cancellationToken = default);
    Task<FileFluentItem> GetAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<FileFluentItem?> FindFileAsync(string fullPath, CancellationToken cancellationToken = default);
    Task<FileFluentItem> GetFileAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<FileFluentItem?> FindDirectoryAsync(string fullPath, CancellationToken cancellationToken = default);
    Task<FileFluentItem> GetDirectoryAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<bool> IsFileExistsAsync(string fullPath, CancellationToken cancellationToken = default);
    Task<bool> IsDirectoryExistsAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(string fullPath, bool forceDeleteBlob = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteDirectoryAsync(string fullPath, bool forceDeleteBlob = false, CancellationToken cancellationToken = default);

    Task<Stream?> GetFileStreamAsync(string fullPath, CancellationToken cancellationToken = default);
    Task<byte[]?> GetFileBytesAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<FileFluentItem> SaveAsync(string fullPath, byte[] bytes, string? mimeType = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);
    Task<FileFluentItem> SaveAsync(string fullPath, Stream stream, string? mimeType = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default);

    Task<FileFluentItem> CreateDirectoryAsync(string fullPath, CancellationToken cancellationToken = default);

    Task<IList<FileFluentItem>> GetFilesAsync(string fullPath, bool includeSubDirectory = false, CancellationToken cancellationToken = default);

    Task MoveAsync(string fullPath, string destinationPath, CancellationToken cancellationToken = default);

    Task RenameAsync(string fullPath, string newName, CancellationToken cancellationToken = default);
}
