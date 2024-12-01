using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

public class FileContainerBasicDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string? Description { get; set; }
    public virtual int FilesCount { get; set; }
}
