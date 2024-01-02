using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.FileManagement.Files;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-management/containers/{containerId}/files")]
public class FileAdminController : FileManagementController, IFileAdminAppService
{
    private readonly IFileAdminAppService _service;

    public FileAdminController(IFileAdminAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet]
    public virtual Task<PagedResultDto<FileDto>> GetListAsync(Guid containerId, FilePagedListRequestDto input)
    {
        return _service.GetListAsync(containerId, input);
    }

    /// <inheritdoc/>
    [HttpGet("{id}")]
    public virtual Task<FileDto> GetAsync(Guid containerId, Guid id)
    {
        return _service.GetAsync(containerId, id);
    }

    /// <inheritdoc/>
    [HttpPost("directories")]
    public virtual Task<FileDto> CreateDirectoryAsync(Guid containerId, FileDirectoryCreateDto input)
    {
        return _service.CreateDirectoryAsync(containerId, input);
    }

    /// <inheritdoc/>
    [HttpPost]
    public virtual Task<FileDto> CreateAsync(Guid containerId, [FromForm] FileCreateDto input)
    {
        return _service.CreateAsync(containerId, input);
    }

    /// <inheritdoc/>
    [NonAction]
    public virtual Task<FileDto> CreateByStreamAsync(Guid containerId, FileCreateByStreamDto input)
    {
        return _service.CreateByStreamAsync(containerId, input);
    }

    /// <inheritdoc/>
    [NonAction]
    public virtual Task<FileDto> CreateByBytesAsync(Guid containerId, FileCreateByBytesDto input)
    {
        return _service.CreateByBytesAsync(containerId, input);
    }

    /// <inheritdoc/>
    [HttpPut("{id}")]
    public virtual Task<FileDto> UpdateAsync(Guid containerId, Guid id, FileUpdateDto input)
    {
        return _service.UpdateAsync(containerId, id, input);
    }

    /// <inheritdoc/>
    [HttpPost("{id}/move")]
    public virtual Task<FileDto> MoveAsync(Guid containerId, Guid id, FileMoveAdminRequestDto input)
    {
        return _service.MoveAsync(containerId, id, input);
    }

    /// <inheritdoc/>
    [HttpGet("{id}/blob")]
    public virtual Task<IRemoteStreamContent?> GetBlobAsync(Guid containerId, Guid id)
    {
        return _service.GetBlobAsync(containerId, id);
    }

    /// <inheritdoc/>
    [NonAction]
    public virtual Task<Stream?> GeBlobStreamAsync(Guid containerId, Guid id)
    {
        return _service.GeBlobStreamAsync(containerId, id);
    }

    /// <inheritdoc/>
    [NonAction]
    public virtual Task<byte[]?> GetBlobBytesAsync(Guid containerId, Guid id)
    {
        return _service.GetBlobBytesAsync(containerId, id);
    }

    /// <inheritdoc/>
    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid containerId, Guid id)
    {
        return _service.DeleteAsync(containerId, id);
    }
}
