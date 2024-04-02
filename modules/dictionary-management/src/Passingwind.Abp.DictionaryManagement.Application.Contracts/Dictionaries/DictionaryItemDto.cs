using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public string? GroupName { get; set; }
    public virtual string DisplayName { get; set; } = null!;
    public virtual int DisplayOrder { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual string? Description { get; set; }
    public virtual string? Value { get; set; }
}
