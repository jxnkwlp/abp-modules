namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupResultDto
{
    public virtual string Name { get; set; } = null!;
    public virtual string DisplayName { get; set; } = null!;
    public virtual string? ParentName { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool IsPublic { get; set; }
}
