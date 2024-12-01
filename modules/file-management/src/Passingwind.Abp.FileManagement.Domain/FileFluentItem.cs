using System;

namespace Passingwind.Abp.FileManagement;

public class FileFluentItem
{
    public FileFluentItem(Guid containerId, Guid id, string fileName, bool isDirectory, string path, long? length = 0, string? hash = null)
    {
        ContainerId = containerId;
        Id = id;
        FileName = fileName;
        IsDirectory = isDirectory;
        Hash = hash;
        Path = path;
        Length = length ?? 0;
    }

    public Guid ContainerId { get; }
    public Guid Id { get; }
    public string FileName { get; }
    public bool IsDirectory { get; }
    public string? Hash { get; }
    public string Path { get; }
    public long Length { get; }

    // public static implicit operator FileFluentItem(FileItem fileItem) => new FileFluentItem(fileItem.ContainerId, fileItem.Id, fileItem.FileName, fileItem.IsDirectory, fileItem.Path.FullPath, fileItem.Length, fileItem.Hash);
}
