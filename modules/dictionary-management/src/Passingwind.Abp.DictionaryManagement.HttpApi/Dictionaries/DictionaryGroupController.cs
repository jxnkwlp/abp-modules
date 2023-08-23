using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

[Area("DictionaryManagement")]
[RemoteService(Name = DictionaryManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/dictionary-management/groups")]
public class DictionaryGroupController : DictionaryManagementController, IDictionaryGroupAppService
{
    private readonly IDictionaryGroupAppService _service;

    public DictionaryGroupController(IDictionaryGroupAppService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<DictionaryGroupDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DictionaryGroupDto>> GetListAsync(DictionaryGroupListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{name}")]
    public virtual Task<DictionaryGroupDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpPost]
    public virtual Task<DictionaryGroupDto> CreateAsync(DictionaryGroupCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{name}")]
    public virtual Task<DictionaryGroupDto> UpdateAsync(string name, DictionaryGroupUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }

    [HttpDelete("{name}")]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }
}
