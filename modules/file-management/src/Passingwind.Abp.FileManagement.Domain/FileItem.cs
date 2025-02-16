using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement;

public class FileItem : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid ContainerId { get; protected set; }

    public virtual Guid ParentId { get; protected set; }

    public virtual bool IsDirectory { get; protected set; }

    public virtual string FileName { get; protected set; } = null!;

    /// <summary>
    ///  The file unique id (global)
    /// </summary>
    public virtual string UniqueId { get; protected set; } = null!;

    public virtual string? MimeType { get; set; }

    public virtual long Length { get; protected set; }

    public virtual string BlobName { get; protected set; } = null!;

    public virtual string? Hash { get; protected set; }

    public virtual int EntityVersion { get; protected set; }

    public virtual List<FileTags> Tags { get; set; } = [];

    public virtual FilePath Path { get; protected set; } = null!;

    protected FileItem()
    {
    }

    public FileItem(
        Guid id,
        Guid containerId,
        bool isDirectory,
        string fileName,
        string blobName,
        Guid? parentId = null,
        string? mimeType = null,
        long? length = null,
        string? hash = null,
        string? uniqueId = null,
        Guid? tenantId = null) : base(id)
    {
        ContainerId = containerId;
        IsDirectory = isDirectory;
        FileName = fileName;
        ParentId = parentId ?? Guid.Empty;
        MimeType = mimeType;
        Length = length ?? 0;
        BlobName = blobName;
        Hash = hash;
        UniqueId = uniqueId ?? id.ToString("N");
        TenantId = tenantId;
    }

    public FileItem SetFileName(string fileName)
    {
        if (FileName == fileName)
        {
            return this;
        }

        var oldName = FileName;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        AddDistributedEvent(new FileNameChangedEvent(Id, ContainerId, ParentId, IsDirectory, UniqueId, fileName, oldName, TenantId));

        return this;
    }

    public FileItem ChangeContainerId(Guid value)
    {
        ContainerId = value;
        return this;
    }

    public FileItem ChangeParentId(Guid parentId)
    {
        ParentId = parentId;
        return this;
    }

    public FileItem SetMimeType(string mimeType)
    {
        MimeType = mimeType;
        return this;
    }

    public FileItem SetLength(long length)
    {
        Length = length;
        return this;
    }

    public FileItem SetUniqueId(string uniqueId)
    {
        UniqueId = uniqueId;
        return this;
    }

    public FileItem SetHash(string hash)
    {
        Hash = hash;
        return this;
    }

    public FileItem AddTags(Dictionary<string, string?> tags)
    {
        foreach (var item in tags)
        {
            Tags.AddIfNotContains(x => x.Name == item.Key, () => new FileTags() { Name = item.Key, Value = item.Value });
        }
        return this;
    }

    public FileItem SetTags(Dictionary<string, string?> tags)
    {
        foreach (var item in tags)
        {
            if (Tags.Any(x => x.Name == item.Key))
            {
                Tags.First(x => x.Name == item.Key).Value = item.Value;
            }
            else
            {
                Tags.AddIfNotContains(x => x.Name == item.Key, () => new FileTags() { Name = item.Key, Value = item.Value });
            }
        }
        return this;
    }

    public FileItem AddTag(string name, string? value)
    {
        Tags.AddIfNotContains(x => x.Name == name, () => new FileTags() { Name = name, Value = value });
        return this;
    }

    public FileItem SetTag(string name, string? value)
    {
        if (Tags.Any(x => x.Name == name))
        {
            Tags.First(x => x.Name == name).Value = value;
        }
        else
        {
            AddTag(name, value);
        }
        return this;
    }

    public string? GetTag(string name)
    {
        return Tags.Find(x => x.Name == name)?.Value;
    }

    public FileItem ClearTags()
    {
        Tags.Clear();

        return this;
    }

    public FileItem SetFullPath(string value)
    {
        Path = new FilePath(Id, value);
        return this;
    }
}
