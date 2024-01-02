using System;

namespace Passingwind.Abp.FileManagement;

public class FileNameChangedEvent
{
    public FileNameChangedEvent(Guid id, Guid containerId, Guid parentId, bool isDirectory, string uniqueId, string fileName, string oldFileName, Guid? tenantId)
    {
        Id = id;
        ContainerId = containerId;
        ParentId = parentId;
        IsDirectory = isDirectory;
        UniqueId = uniqueId;
        FileName = fileName;
        OldFileName = oldFileName;
        TenantId = tenantId;
    }

    public Guid Id { get; }

    public virtual Guid ContainerId { get; }

    public virtual Guid ParentId { get; }

    public virtual bool IsDirectory { get; }

    public virtual string UniqueId { get; } = null!;
    public virtual string FileName { get; } = null!;
    public virtual string OldFileName { get; } = null!;
    public virtual Guid? TenantId { get; }
}
