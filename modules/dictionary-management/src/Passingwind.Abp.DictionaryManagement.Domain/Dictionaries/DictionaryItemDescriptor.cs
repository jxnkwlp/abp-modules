namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemDescriptor
{
    /// <summary>
    ///  The unique name for dictionary
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    ///  The display name for dictionary
    /// </summary>
    public string DisplayName { get; set; } = null!;
    public int DisplayOrder { get; set; } = 100;
    /// <summary>
    ///  identification enabled this item
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    /// <summary>
    ///  The group name for dictionary
    /// </summary>
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
}
