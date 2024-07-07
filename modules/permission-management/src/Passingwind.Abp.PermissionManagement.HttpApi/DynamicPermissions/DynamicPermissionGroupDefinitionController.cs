using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/permission-management/dynamic/definitions/groups")]
public class DynamicPermissionGroupDefinitionController : PermissionManagementController, IDynamicPermissionGroupDefinitionAppService
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
