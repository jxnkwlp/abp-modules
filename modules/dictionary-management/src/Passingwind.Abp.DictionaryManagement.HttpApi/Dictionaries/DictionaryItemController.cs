using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

[Area(DictionaryManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = DictionaryManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/dictionary-management/items")]
public class DictionaryItemController : DictionaryManagementController, IDictionaryItemAppService
{
    private readonly IDictionaryItemAppService _service;

    public DictionaryItemController(IDictionaryItemAppService service)
    {
        _service = service;
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<DictionaryItemDto>> GetAllListAsync(DictionaryItemListRequestDto input)
    {
        return _service.GetAllListAsync(input);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DictionaryItemDto>> GetListAsync(DictionaryItemPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{name}")]
    public virtual Task<DictionaryItemDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpPost]
    public virtual Task<DictionaryItemDto> CreateAsync(DictionaryItemCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{name}")]
    public virtual Task<DictionaryItemDto> UpdateAsync(string name, DictionaryItemUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }

    [HttpDelete("{name}")]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }
}
