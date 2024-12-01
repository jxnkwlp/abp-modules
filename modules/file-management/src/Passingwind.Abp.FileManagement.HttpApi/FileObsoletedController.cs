using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

[Obsolete("The obsoleted controller")]
[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-management")]
public class FileObsoletedController : FileManagementController
{
    private readonly IFileAppService _service;

    public FileObsoletedController(IFileAppService service)
    {
        _service = service;
    }

    [HttpGet("{containerName}")]
    public virtual Task<PagedResultDto<FileDto>> GetListAsync(string containerName, FilePagedListRequestDto input)
    {
        return _service.GetListAsync(containerName, input);
    }

    [HttpGet("{containerName}/{id}")]
    public virtual Task<FileDto> GetAsync(string containerName, Guid id)
    {
        return _service.GetAsync(containerName, id);
    }

    [HttpGet("{containerName}/{id}/blob")]
    public virtual Task<IRemoteStreamContent?> GetBlobAsync(string containerName, Guid id)
    {
        return _service.GetBlobAsync(containerName, id);
    }

    [HttpPost("{containerName}")]
    [Consumes("multipart/form-data")]
    public virtual Task<FileDto> CreateAsync(string containerName, [FromForm] FileCreateDto input)
    {
        return _service.CreateAsync(containerName, input);
    }

    [HttpPut("{containerName}/{id}")]
    public virtual Task<FileDto> RenameAsync(string containerName, Guid id, FileUpdateDto input)
    {
        return _service.RenameAsync(containerName, id, input);
    }

    [HttpPost("{containerName}/directory")]
    public virtual Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input)
    {
        return _service.CreateDirectoryAsync(containerName, input);
    }
}
