namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryResultDto
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Value { get; set; }
}
