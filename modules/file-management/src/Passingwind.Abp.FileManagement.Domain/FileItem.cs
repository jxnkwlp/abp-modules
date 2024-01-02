using System;
using System.Collections.Generic;
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

    public int EntityVersion { get; protected set; }

    public virtual List<FileTags> Tags { get; protected set; } = [];

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

    public void SetFileName(string fileName)
    {
        if (FileName == fileName)
        {
            return;
        }

        var oldName = FileName;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        AddDistributedEvent(new FileNameChangedEvent(Id, ContainerId, ParentId, IsDirectory, UniqueId, fileName, oldName, TenantId));
    }

    public void ChangeContainerId(Guid value)
    {
        ContainerId = value;
    }

    public void ChangeParentId(Guid parentId)
    {
        ParentId = parentId;
    }

    public void SetMimeType(string mimeType)
    {
        MimeType = mimeType;
    }

    public void SetLength(long length)
    {
        Length = length;
    }

    public void SetUniqueId(string uniqueId)
    {
        UniqueId = uniqueId;
    }

    public void SetHash(string hash)
    {
        Hash = hash;
    }

    public void AddTags(params string[] tags)
    {
        foreach (var tag in tags)
        {
            Tags.AddIfNotContains(x => x.Tags == tag, () => new FileTags() { Tags = tag });
        }
    }
}
