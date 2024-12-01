using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-management/containers")]
public class FileContainerAdminController : FileManagementController, IFileContainerAdminAppService
{
    private readonly IFileContainerAdminAppService _service;

    public FileContainerAdminController(IFileContainerAdminAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerAdminListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<FileContainerDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<FileContainerDto> CreateAsync(FileContainerAdminCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<FileContainerDto> UpdateAsync(Guid id, FileContainerAdminUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<FileContainerDto>> GetAllListAsync()
    {
        return _service.GetAllListAsync();
    }
}
