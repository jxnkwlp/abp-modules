namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupDescriptor
{
    /// <summary>
    ///  The unique name for group
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    ///  The display name for group
    /// </summary>
    public string DisplayName { get; set; } = null!;
    /// <summary>
    ///  The group parent name, can be null
    /// </summary>
    public string? ParentName { get; set; }
    public string? Description { get; set; }
    /// <summary>
    ///  Is public for this group
    ///  if true, anonymous can be view
    ///  otherwise, only authentication use can be view
    /// </summary>
    public bool IsPublic { get; set; }
}
