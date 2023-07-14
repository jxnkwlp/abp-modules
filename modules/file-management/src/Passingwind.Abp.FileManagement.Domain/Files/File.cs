using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement.Files;

public class File : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid ContainerId { get; protected set; }

    public virtual Guid? ParentId { get; protected set; }

    public virtual bool IsDirectory { get; protected set; }

    public virtual string FileName { get; protected set; } = null!;

    /// <summary>
    ///  The file unique id (global)
    /// </summary>
    public virtual string UniqueId { get; protected set; } = null!;

    public virtual string? MimeType { get; protected set; }

    public virtual long Length { get; protected set; }

    public virtual string BlobName { get; protected set; } = null!;

    public virtual string? Hash { get; protected set; }

    protected File()
    {
    }

    public File(
        Guid id,
        Guid containerId,
        bool isDirectory,
        string fileName,
        string? mimeType,
        long? length,
        string blobName,
        string? hash,
        string? uniqueId = null) : base(id)
    {
        ContainerId = containerId;
        IsDirectory = isDirectory;
        FileName = fileName;
        MimeType = mimeType;
        Length = length ?? 0;
        BlobName = blobName;
        Hash = hash;
        UniqueId = uniqueId ?? id.ToString("N").Substring(8, 16); // TODO
    }

    public void SetFileName(string fileName)
    {
        if (fileName == null)
            throw new ArgumentNullException(nameof(fileName));

        FileName = fileName;
    }

    public void ChangeParentId(Guid? parentId)
    {
        ParentId = parentId;
    }

    public void UpdateMimeType(string mimeType)
    {
        MimeType = mimeType;
    }

    public void UpdateLength(long length)
    {
        Length = length;
    }

    public void SetUniqueId(string uniqueId)
    {
        UniqueId = uniqueId;
    }
}
