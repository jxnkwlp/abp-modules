using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual string DisplayName { get; set; } = null!;
    public virtual string? ParentName { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool IsPublic { get; set; }
}
