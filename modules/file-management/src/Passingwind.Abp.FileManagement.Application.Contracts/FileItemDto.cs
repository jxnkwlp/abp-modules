using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

public class FileItemDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual bool IsDirectory { get; set; }
    public virtual string FileName { get; set; } = null!;
    public virtual string? MimeType { get; set; }
    public virtual long Length { get; set; }
    public virtual string? Hash { get; set; }
    public virtual string UniqueId { get; set; } = null!;
    public virtual Guid? ParentId { get; set; }
    public virtual List<string> Tags { get; set; } = null!;
}
