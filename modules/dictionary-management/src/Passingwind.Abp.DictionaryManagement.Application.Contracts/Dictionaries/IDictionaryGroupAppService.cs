using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public interface IDictionaryGroupAppService : IApplicationService
{
    Task<ListResultDto<DictionaryGroupDto>> GetAllListAsync();

    Task<PagedResultDto<DictionaryGroupDto>> GetListAsync(DictionaryGroupListRequestDto input);

    Task<DictionaryGroupDto> GetAsync(string name);

    Task<DictionaryGroupDto> CreateAsync(DictionaryGroupCreateDto input);

    Task<DictionaryGroupDto> UpdateAsync(string name, DictionaryGroupUpdateDto input);

    Task DeleteAsync(string name);
}
