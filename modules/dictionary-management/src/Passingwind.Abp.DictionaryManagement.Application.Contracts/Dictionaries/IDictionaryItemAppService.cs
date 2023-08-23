using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public interface IDictionaryItemAppService : IApplicationService
{
    Task<ListResultDto<DictionaryItemDto>> GetAllListAsync(DictionaryItemListRequestDto input);

    Task<PagedResultDto<DictionaryItemDto>> GetListAsync(DictionaryItemPagedListRequestDto input);

    Task<DictionaryItemDto> GetAsync(string name);

    Task<DictionaryItemDto> CreateAsync(DictionaryItemCreateDto input);

    Task<DictionaryItemDto> UpdateAsync(string name, DictionaryItemUpdateDto input);

    Task DeleteAsync(string name);
}
