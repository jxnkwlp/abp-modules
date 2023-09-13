using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileManager : IDomainService
{
    Task<File> FindFileAsync(FileContainer container, string fileName, Guid? parentId, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Override an exist file with bytes
    /// </summary>
    /// <param name="container"></param>
    /// <param name="file"></param>
    /// <param name="bytes"></param>
    /// <param name="cancellationToken"></param>
    Task<File> UpdateFileAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Create an new file with bytes
    /// </summary>
    /// <param name="container"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <param name="bytes"></param>
    /// <param name="parentId"></param>
    /// <param name="cancellationToken"></param>
    Task<File> CreateFileAsync(FileContainer container, string fileName, string mimeType, byte[] bytes, Guid? parentId = null, CancellationToken cancellationToken = default);

    Task<File> CreateDirectoryAsync(FileContainer container, string name, Guid? parentId, CancellationToken cancellationToken = default);

    Task<File> ChangeNameAsync(FileContainer container, File file, string newName, Guid? parentId, CancellationToken cancellationToken = default);

    Task<byte[]> GetFileBytesAsync(FileContainer container, File file, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamAsync(FileContainer container, File file, CancellationToken cancellationToken = default);

    Task<bool> IsFileExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default);
    Task<bool> IsDirectoryExistsAsync(FileContainer container, File file, CancellationToken cancellationToken = default);

    Task SaveBlobAsync(FileContainer container, File file, byte[] bytes, CancellationToken cancellationToken = default);
    Task SaveBlobAsync(FileContainer container, File file, Stream stream, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Delete an file
    /// </summary>
    /// <param name="container"></param>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(FileContainer container, File file, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Delete all files
    /// </summary>
    /// <param name="container"></param>
    /// <param name="cancellationToken"></param>
    Task ClearContainerFilesAsync(FileContainer container, CancellationToken cancellationToken = default);

    Task<byte[]> GetFileBytesByFileIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default);
    Task<Stream?> GetFileSteamByFileIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default);

    Task<File> GetByIdAsync(string containerName, Guid id, CancellationToken cancellationToken = default);
}
