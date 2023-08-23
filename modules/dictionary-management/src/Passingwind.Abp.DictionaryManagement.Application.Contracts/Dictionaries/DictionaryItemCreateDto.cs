using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemCreateDto : DictionaryItemUpdateDto
{
    [Required]
    [MaxLength(32)]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string Name { get; set; } = null!;
}
