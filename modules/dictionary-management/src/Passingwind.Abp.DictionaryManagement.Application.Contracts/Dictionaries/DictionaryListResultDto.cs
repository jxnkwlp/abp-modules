using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryListResultDto : ListResultDto<DictionaryResultDto>
{
    public DictionaryListResultDto()
    {
    }

    public DictionaryListResultDto(IReadOnlyList<DictionaryResultDto> items) : base(items)
    {
    }
}
