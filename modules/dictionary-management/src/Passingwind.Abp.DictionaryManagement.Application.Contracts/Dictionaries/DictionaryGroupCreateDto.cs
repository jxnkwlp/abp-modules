using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupCreateDto : DictionaryGroupUpdateDto
{
    [Required]
    [MaxLength(32)]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string Name { get; set; } = null!;
    [MaxLength(32)]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string? ParentName { get; set; }
}
