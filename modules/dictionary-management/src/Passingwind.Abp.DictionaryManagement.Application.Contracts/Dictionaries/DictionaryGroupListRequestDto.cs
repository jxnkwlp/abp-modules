using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupListRequestDto : PagedResultRequestDto
{
    public string? Filter { get; set; }
    public string? ParentName { get; set; }
}
