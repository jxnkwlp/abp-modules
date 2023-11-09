using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

[Area(DynamicPermissionManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = DynamicPermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/permission-management/definitions")]
public class DynamicPermissionDefinitionController : DynamicPermissionManagementController, IDynamicPermissionDefinitionAppService
{
    private readonly IDynamicPermissionDefinitionAppService _service;

    public DynamicPermissionDefinitionController(IDynamicPermissionDefinitionAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DynamicPermissionDefinitionDto>> GetListAsync(DynamicPermissionDefinitionPagedListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<DynamicPermissionDefinitionDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<DynamicPermissionDefinitionDto> CreateAsync(DynamicPermissionDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<DynamicPermissionDefinitionDto> UpdateAsync(Guid id, DynamicPermissionDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<DynamicPermissionDefinitionDto>> GetAllListAsync(DynamicPermissionDefinitionListRequestDto input)
    {
        return _service.GetAllListAsync(input);
    }
}
