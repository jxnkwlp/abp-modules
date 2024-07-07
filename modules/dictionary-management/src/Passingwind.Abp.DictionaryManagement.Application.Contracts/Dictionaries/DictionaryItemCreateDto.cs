using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemCreateDto : DictionaryItemUpdateDto
{
    [Required]
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxNameLength))]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public virtual string Name { get; set; } = null!;
}
