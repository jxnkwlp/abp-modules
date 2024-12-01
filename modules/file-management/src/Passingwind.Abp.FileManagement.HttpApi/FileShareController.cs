using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/files/{containerName}")]
public class FileShareController : FileManagementController, IFileShareAppService
{
    private readonly IFileShareAppService _service;

    public FileShareController(IFileShareAppService service)
    {
        _service = service;
    }

    [HttpGet("shares")]
    public virtual Task<PagedResultDto<FileShareResultDto>> GetListAsync(string containerName, FileShareListRequestDto input)
    {
        return _service.GetListAsync(containerName, input);
    }

    [HttpPost("shares/files/{fileId}")]
    public virtual Task<FileShareResultDto> CreateAsync(string containerName, Guid fileId, FileShareCreateRequestDto input)
    {
        return _service.CreateAsync(containerName, fileId, input);
    }

    [HttpDelete("shares/{id}")]
    public virtual Task DeleteAsync(string containerName, Guid id)
    {
        return _service.DeleteAsync(containerName, id);
    }

    [HttpGet("shares/{id}")]
    public virtual Task<FileShareResultDto> GetAsync(string containerName, Guid id)
    {
        return _service.GetAsync(containerName, id);
    }

    [HttpPost("shares/token/{token}/verify")]
    public virtual Task<FileShareResultDto> VerifyTokenAsync(string containerName, string token)
    {
        return _service.VerifyTokenAsync(containerName, token);
    }

    [HttpGet("shares/token/{token}/blob")]
    public virtual Task<IRemoteStreamContent?> GetBlobAsync(string containerName, string token)
    {
        return _service.GetBlobAsync(containerName, token);
    }

    [NonAction]
    public virtual Task<byte[]?> GetBytesAsync(string containerName, string token)
    {
        return _service.GetBytesAsync(containerName, token);
    }

    [NonAction]
    public virtual Task<Stream?> GetStreamAsync(string containerName, string token)
    {
        return _service.GetStreamAsync(containerName, token);
    }
}
