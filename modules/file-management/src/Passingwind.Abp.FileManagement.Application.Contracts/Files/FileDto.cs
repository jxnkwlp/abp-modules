using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FileDto : AuditedEntityDto<Guid>
{
    public virtual bool IsDirectory { get; set; }
    public virtual string FileName { get; set; } = null!;
    public virtual string? MimeType { get; set; }
    public virtual long Length { get; set; }
    public virtual string? Hash { get; set; }
    public virtual string UniqueId { get; set; } = null!;
}
