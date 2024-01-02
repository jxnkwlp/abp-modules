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
[Route("api/files")]
public class FileShareController : FileManagementController, IFileShareAppService
{
    private readonly IFileShareAppService _service;

    public FileShareController(IFileShareAppService service)
    {
        _service = service;
    }

    [HttpPost("{containerName}/files/{id}/shares")]
    public virtual Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input)
    {
        return _service.CreateAsync(containerName, id, input);
    }

    [HttpDelete("shares/{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("shares/{id}")]
    public virtual Task<FileShareResultDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet("shares/token/{token}/blob")]
    public virtual Task<IRemoteStreamContent?> GetBlobAsync(string token)
    {
        return _service.GetBlobAsync(token);
    }

    [NonAction]
    public virtual Task<byte[]?> GetBytesAsync(string token)
    {
        return _service.GetBytesAsync(token);
    }

    [HttpGet("shares")]
    public virtual Task<PagedResultDto<FileShareResultDto>> GetListAsync(string containerName, FileShareListRequestDto input)
    {
        return _service.GetListAsync(containerName, input);
    }

    [NonAction]
    public virtual Task<Stream?> GetStreamAsync(string token)
    {
        return _service.GetStreamAsync(token);
    }

    [HttpPost("shares/token/{token}/verify")]
    public virtual Task<FileShareResultDto> VerifyTokenAsync(string token)
    {
        return _service.VerifyTokenAsync(token);
    }
}
