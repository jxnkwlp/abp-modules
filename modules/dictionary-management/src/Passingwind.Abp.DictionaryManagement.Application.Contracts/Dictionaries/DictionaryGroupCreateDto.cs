using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupCreateDto : DictionaryGroupUpdateDto
{
    [Required]
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxNameLength))]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string Name { get; set; } = null!;
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxNameLength))]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string? ParentName { get; set; }
}
