using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItemPagedListRequestDto : PagedResultRequestDto
{
    public string? Filter { get; set; }
    public string? GroupName { get; set; }
}
