using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string? Description { get; set; }
    public virtual FileAccessMode AccessMode { get; set; }
    public virtual string AccessModeDescription => AccessMode.ToString();
    public virtual long? MaximumEachFileSize { get; set; }
    public virtual int? MaximumFileQuantity { get; set; }
    public virtual FileOverrideBehavior? OverrideBehavior { get; set; }
    public virtual string? OverrideBehaviorDescription => OverrideBehavior?.ToString();
    public virtual bool AllowAnyFileExtension { get; set; }
    public virtual string? AllowedFileExtensions { get; set; }
    public virtual string? ProhibitedFileExtensions { get; set; }
    public virtual int FilesCount { get; set; }
    public virtual bool AutoDeleteBlob { get; set; }
}
