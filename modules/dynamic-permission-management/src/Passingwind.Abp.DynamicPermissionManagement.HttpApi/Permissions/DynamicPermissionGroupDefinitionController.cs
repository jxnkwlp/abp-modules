using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

[Area(DynamicPermissionManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = DynamicPermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/permission-management/definitions/groups")]
public class DynamicPermissionGroupDefinitionController : DynamicPermissionManagementController, IDynamicPermissionGroupDefinitionAppService
{
    private readonly IDynamicPermissionGroupDefinitionAppService _service;

    public DynamicPermissionGroupDefinitionController(IDynamicPermissionGroupDefinitionAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DynamicPermissionGroupDefinitionDto>> GetListAsync(DynamicPermissionGroupDefinitionListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<DynamicPermissionGroupDefinitionDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<DynamicPermissionGroupDefinitionDto> CreateAsync(DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<DynamicPermissionGroupDefinitionDto> UpdateAsync(Guid id, DynamicPermissionGroupDefinitionCreateOrUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<DynamicPermissionGroupDefinitionDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }
}
