using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.ApiKey;

[ControllerName("ApiKey")]
[Area(ApiKeyRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = ApiKeyRemoteServiceConsts.RemoteServiceName)]
[Route("api/account/api-keys")]
public class ApiKeyRecordController : ApiKeyControllerBase, IApiKeyRecordAppService
{
    private readonly IApiKeyRecordAppService _service;

    public ApiKeyRecordController(IApiKeyRecordAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<PagedResultDto<ApiKeyRecordDto>> GetListAsync(ApiKeyRecordListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    /// <inheritdoc/>
    [HttpPost]
    public virtual Task<ApiKeyRecordDto> CreateAsync(ApiKeyRecordCreateOrUpdateDto input)
    {
        return _service.CreateAsync(input);
    }

    /// <inheritdoc/>
    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }
}
