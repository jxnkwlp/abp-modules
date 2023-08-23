using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupUpdateDto : ExtensibleObject
{
    [Required]
    [MaxLength(64)]
    public virtual string DisplayName { get; set; } = null!;
    public virtual string? Description { get; set; }
    public virtual bool IsPublic { get; set; }
}
