using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroupListResultDto : ListResultDto<DictionaryGroupResultDto>
{
    public DictionaryGroupListResultDto()
    {
    }

    public DictionaryGroupListResultDto(IReadOnlyList<DictionaryGroupResultDto> items) : base(items)
    {
    }
}
