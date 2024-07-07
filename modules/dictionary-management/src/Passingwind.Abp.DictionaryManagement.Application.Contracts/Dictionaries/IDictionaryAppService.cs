using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public interface IDictionaryAppService : IApplicationService
{
    Task<DictionaryResultDto> GetAsync(string name);
    Task<DictionaryListResultDto> GetListByGroupAsync(string groupName);
    Task<DictionaryGroupListResultDto> GetGroupListAsync(string? parentName = null);
}
