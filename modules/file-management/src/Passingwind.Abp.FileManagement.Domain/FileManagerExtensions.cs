using System;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.Abp.FileManagement;

public static class FileManagerExtensions
{
    public static async Task EnsureDirectoryExistsAsync(this IFileManager fileManager, string container, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (!await fileManager.IsDirectoryExistsAsync(container, fileName, parentId, cancellationToken: cancellationToken))
        {
            await fileManager.CreateDirectoryAsync(container, fileName, parentId, cancellationToken: cancellationToken);
        }
    }

    public static async Task EnsureDirectoryExistsAsync(this IFileManager fileManager, Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        if (!await fileManager.IsDirectoryExistsAsync(containerId, fileName, parentId, cancellationToken: cancellationToken))
        {
            await fileManager.CreateDirectoryAsync(containerId, fileName, parentId, cancellationToken: cancellationToken);
        }
    }
}
