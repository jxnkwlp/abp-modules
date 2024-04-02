using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupUpdateDto : ExtensibleObject
{
    [Required]
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxDisplayNameLength))]
    public virtual string DisplayName { get; set; } = null!;
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxDescriptionLength))]
    public virtual string? Description { get; set; }
    public virtual bool IsPublic { get; set; }
}
