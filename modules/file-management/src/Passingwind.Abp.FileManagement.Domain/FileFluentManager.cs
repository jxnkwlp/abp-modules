using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Passingwind.Abp.FileManagement;

public class FileFluentManager : IFileFluentManager, IScopedDependency
{
    protected ILogger<FileFluentManager> Logger { get; }
    protected IFileItemRepository FileRepository { get; }
    protected IFileItemManager FileManager { get; }
    protected IFileContainerRepository FileContainerRepository { get; }

    public FileFluentManager(ILogger<FileFluentManager> logger, IFileItemRepository fileRepository, IFileItemManager fileManager, IFileContainerRepository fileContainerRepository)
    {
        Logger = logger;
        FileRepository = fileRepository;
        FileManager = fileManager;
        FileContainerRepository = fileContainerRepository;
    }

    private static FileFluentItem? ToFluentItem(FileItem? fileItem)
    {
        if (fileItem == null)
            return null;

        return new FileFluentItem(fileItem.ContainerId, fileItem.Id, fileItem.FileName, fileItem.IsDirectory, fileItem.Path.FullPath, fileItem.Length, fileItem.Hash);
    }

    public virtual async Task<FileFluentItem> CreateDirectoryAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        if (await IsDirectoryExistsAsync(fullPath, cancellationToken))
        {
            throw new UserFriendlyException("The directory path is exists.");
        }

        var filePath = FilePathParse.FromDirectoryPath(fullPath);

        Guid parentId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(filePath.DirectoryPath))
        {
            if (!await IsDirectoryExistsAsync(filePath.GetDirectory(), cancellationToken))
            {
                var parent = await CreateDirectoryAsync(filePath.GetDirectory(), cancellationToken);
                parentId = parent.Id;
            }
            else
            {
                var parent = await GetDirectoryAsync(filePath.GetDirectory(), cancellationToken);
                parentId = parent.Id;
            }
        }

        return ToFluentItem(await FileManager.CreateDirectoryAsync(filePath.Container, filePath.Name, parentId, cancellationToken))!;
    }

    public virtual async Task<bool> DeleteFileAsync(string fullPath, bool forceDeleteBlob = false, CancellationToken cancellationToken = default)
    {
        var item = await FindFileAsync(fullPath, cancellationToken);
        if (item != null)
        {
            await FileManager.DeleteAsync(item.Id, forceDeleteBlob, cancellationToken);

            return true;
        }

        return false;
    }

    public virtual async Task<bool> DeleteDirectoryAsync(string fullPath, bool forceDeleteBlob = false, CancellationToken cancellationToken = default)
    {
        var item = await FindDirectoryAsync(fullPath, cancellationToken);
        if (item != null)
        {
            await FileManager.DeleteAsync(item.Id, forceDeleteBlob, cancellationToken);

            return true;
        }

        return false;
    }

    public virtual async Task<FileFluentItem?> FindAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);

        var container = await FileContainerRepository.FindByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            return null;

        return ToFluentItem(await FileRepository.FindByPathAsync(container.Id, filePath.FullName, cancellationToken));
    }

    public virtual async Task<FileFluentItem?> FindDirectoryAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromDirectoryPath(fullPath);

        var container = await FileContainerRepository.FindByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            return null;

        var item = await FileRepository.FindByPathAsync(container.Id, filePath.FullName, cancellationToken);

        if (item?.IsDirectory == true)
            return ToFluentItem(item);

        return null;
    }

    public virtual async Task<FileFluentItem?> FindFileAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);

        var container = await FileContainerRepository.FindByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            return null;

        var item = await FileRepository.FindByPathAsync(container.Id, filePath.FullName, cancellationToken);

        if (item?.IsDirectory == true)
            return null;

        return ToFluentItem(item);
    }

    public virtual async Task<FileFluentItem> GetAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = fullPath.EndsWith("/") ? FilePathParse.FromDirectoryPath(fullPath) : FilePathParse.FromFilePath(fullPath);

        var container = await FileContainerRepository.GetByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            throw new UserFriendlyException($"The container name '{container}' is not exists.");

        return ToFluentItem(await FileRepository.GetByPathAsync(container.Id, filePath.FullName, cancellationToken))!;
    }

    public virtual async Task<FileFluentItem> GetDirectoryAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromDirectoryPath(fullPath);

        var container = await FileContainerRepository.GetByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            throw new UserFriendlyException($"The container name '{container}' is not exists.");

        var item = await FileRepository.GetByPathAsync(container.Id, filePath.FullName, cancellationToken);

        if (item.IsDirectory)
            return ToFluentItem(item)!;

        throw new UserFriendlyException("The file is not exists.");
    }

    public virtual async Task<FileFluentItem> GetFileAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);

        var container = await FileContainerRepository.GetByNameAsync(filePath.Container, cancellationToken);

        if (container == null)
            throw new UserFriendlyException($"The container name '{container}' is not exists.");

        var item = await FileRepository.GetByPathAsync(container.Id, filePath.FullName, cancellationToken);

        if (item.IsDirectory)
            throw new UserFriendlyException("The file is not exists.");

        return ToFluentItem(item)!;
    }

    public virtual async Task<byte[]?> GetFileBytesAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);
        var file = await GetFileAsync(fullPath, cancellationToken);

        return await FileManager.GetFileBytesAsync(file.Id, cancellationToken);
    }

    public virtual async Task<Stream?> GetFileStreamAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);
        var file = await GetFileAsync(fullPath, cancellationToken);

        return await FileManager.GetFileSteamAsync(file.Id, cancellationToken);
    }

    public virtual async Task<bool> IsDirectoryExistsAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var FileFluentItem = await FindDirectoryAsync(fullPath, cancellationToken);

        return FileFluentItem != null;
    }

    public virtual async Task<bool> IsFileExistsAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        var FileFluentItem = await FindFileAsync(fullPath, cancellationToken);

        return FileFluentItem != null;
    }

    public virtual async Task<FileFluentItem> SaveAsync(string fullPath, byte[] bytes, string? mimeType = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);

        Guid parentId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(filePath.DirectoryPath))
        {
            var dir = await FindDirectoryAsync(filePath.GetDirectory(), cancellationToken);

            if (dir == null)
            {
                dir = await CreateDirectoryAsync(filePath.GetDirectory(), cancellationToken);
            }
            parentId = dir.Id;
        }

        return ToFluentItem(await FileManager.SaveAsync(filePath.Container, filePath.Name, bytes, mimeType, parentId: parentId, ignoreCheck: ignoreCheck, overrideExisting: overrideExisting, cancellationToken: cancellationToken))!;
    }

    public virtual async Task<FileFluentItem> SaveAsync(string fullPath, Stream stream, string? mimeType = null, bool ignoreCheck = false, bool overrideExisting = false, CancellationToken cancellationToken = default)
    {
        var filePath = FilePathParse.FromFilePath(fullPath);

        Guid parentId = Guid.Empty;

        if (!string.IsNullOrWhiteSpace(filePath.DirectoryPath))
        {
            var dir = await FindDirectoryAsync(filePath.GetDirectory(), cancellationToken);

            if (dir == null)
            {
                dir = await CreateDirectoryAsync(filePath.GetDirectory(), cancellationToken);
            }
            parentId = dir.Id;
        }

        return ToFluentItem(await FileManager.SaveAsync(filePath.Container, filePath.Name, stream, mimeType, parentId: parentId, ignoreCheck: ignoreCheck, overrideExisting: overrideExisting, cancellationToken: cancellationToken))!;
    }

    public virtual async Task<IList<FileFluentItem>> GetFilesAsync(string fullPath, bool includeSubDirectory = false, CancellationToken cancellationToken = default)
    {
        var item = await GetDirectoryAsync(fullPath, cancellationToken);

        var list = await FileManager.GetFilesAsync(item.ContainerId, item.Id, includeSubDirectory, cancellationToken);

        return list.ConvertAll(x => ToFluentItem(x)!);
    }

    public virtual async Task MoveAsync(string fullPath, string destinationPath, CancellationToken cancellationToken = default)
    {
        var item = await GetAsync(fullPath, cancellationToken);
        var fileItem = await FileRepository.GetAsync(item.Id, cancellationToken: cancellationToken);

        var targetFilePath = FilePathParse.FromFilePath(destinationPath);

        Guid targetParentId = Guid.Empty;
        if (!string.IsNullOrWhiteSpace(targetFilePath.DirectoryPath))
        {
            var targetParent = await FindDirectoryAsync(targetFilePath.GetDirectory(), cancellationToken);

            if (targetParent == null)
            {
                targetParentId = (await CreateDirectoryAsync(targetFilePath.GetDirectory(), cancellationToken)).Id;
            }
            else
            {
                targetParentId = targetParent.Id;
            }
        }

        await FileManager.MoveAsync(fileId: fileItem.Id, targetFileName: targetFilePath.Name, targetParentId: targetParentId, cancellationToken: cancellationToken);
    }

    public virtual async Task RenameAsync(string fullPath, string newName, CancellationToken cancellationToken = default)
    {
        var item = await GetAsync(fullPath, cancellationToken);

        var fileItem = await FileRepository.GetAsync(item.Id, cancellationToken: cancellationToken);

        await FileManager.ChangeNameAsync(fileItem.Id, newName, cancellationToken);
    }
}
