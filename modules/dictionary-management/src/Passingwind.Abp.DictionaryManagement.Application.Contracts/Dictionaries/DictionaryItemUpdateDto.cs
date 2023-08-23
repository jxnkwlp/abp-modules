using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemUpdateDto : ExtensibleObject
{
    [Required]
    [MaxLength(32)]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public string GroupName { get; set; } = null!;
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
    public virtual int DisplayOrder { get; set; }
    public virtual bool IsEnabled { get; set; }
    public virtual string? Description { get; set; }
    public string? Value { get; set; }
}
