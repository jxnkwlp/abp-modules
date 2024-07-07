using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemUpdateDto : ExtensibleObject
{
    [Required]
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxNameLength))]
    [RegularExpression("^[A-Za-z0-9_\\-\\.]+$")]
    public string GroupName { get; set; } = null!;
    [Required]
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxDisplayNameLength))]
    public virtual string DisplayName { get; set; } = null!;
    public virtual int DisplayOrder { get; set; } = 100;
    public virtual bool IsEnabled { get; set; } = true;
    [DynamicMaxLength(typeof(DictionaryManagementConsts), nameof(DictionaryManagementConsts.MaxDescriptionLength))]
    public virtual string? Description { get; set; }
    public string? Value { get; set; }
}
