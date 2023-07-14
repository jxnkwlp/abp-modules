using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/files/containers")]
public class FileContainerController : FileManagementController, IFileContainerAppService
{
    private readonly IFileContainerAppService _service;

    public FileContainerController(IFileContainerAppService service)
    {
        _service = service;
    }

    [HttpGet()]
    public virtual Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerListRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<FileContainerDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpPost()]
    public virtual Task<FileContainerDto> CreateAsync(FileContainerCreateOrUpdateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<FileContainerDto> UpdateAsync(Guid id, FileContainerCreateOrUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("by-name/{name}")]
    public Task<FileContainerDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }
}
