﻿using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement;

public class FileContainer : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual string Name { get; protected set; } = null!;

    /// <summary>
    ///  How to access files under the container
    /// </summary>
    public virtual FileAccessMode AccessMode { get; protected set; }

    public virtual string? Description { get; set; }

    /// <summary>
    ///  maximun length for single file (KB)
    /// </summary>
    public virtual long MaximumEachFileSize { get; set; } = long.MaxValue;

    /// <summary>
    ///  maximun files count for this container
    /// </summary>
    public virtual int MaximumFileQuantity { get; set; } = int.MaxValue;

    public virtual FileOverrideBehavior OverrideBehavior { get; set; }

    /// <summary>
    ///  Allow any file extensions
    /// </summary>
    public virtual bool AllowAnyFileExtension { get; set; }

    public virtual string? AllowedFileExtensions { get; set; }

    public virtual string? ProhibitedFileExtensions { get; set; }

    public virtual bool AutoDeleteBlob { get; set; }

    public virtual int FilesCount { get; protected set; }

    public int EntityVersion { get; protected set; }

    public List<FileContainerAccess> Accesses { get; set; } = [];

    protected FileContainer()
    {
    }

    public FileContainer(Guid id, string name, FileAccessMode accessMode) : base(id)
    {
        Name = name;
        AccessMode = accessMode;
    }

    public void SetAccessMode(FileAccessMode accessMode)
    {
        AccessMode = accessMode;
    }

    public string[]? GetAllowedFileExtensions()
    {
        return AllowedFileExtensions?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string[]? GetProhibitedFileExtensions()
    {
        return ProhibitedFileExtensions?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public void SetFilesCount(int count)
    {
        FilesCount = count;
    }
}
