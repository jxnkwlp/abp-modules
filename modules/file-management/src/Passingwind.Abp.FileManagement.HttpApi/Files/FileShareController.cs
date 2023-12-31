﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

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

    [HttpPost("{containerName}/{id}/share")]
    public virtual Task<FileShareResultDto> CreateAsync(string containerName, Guid id, FileShareCreateRequestDto input)
    {
        return _service.CreateAsync(containerName, id, input);
    }

    [HttpGet("share/{token}")]
    public virtual Task<FileShareResultDto> GetAsync(string token)
    {
        return _service.GetAsync(token);
    }

    [HttpGet("share/{token}/blob")]
    public virtual Task<IRemoteStreamContent?> GetBlobAsync(string token)
    {
        return _service.GetBlobAsync(token);
    }

    [NonAction]
    public virtual Task<byte[]?> GetBytesAsync(string token)
    {
        return _service.GetBytesAsync(token);
    }

    [NonAction]
    public virtual Task<Stream?> GetStreamAsync(string token)
    {
        return _service.GetStreamAsync(token);
    }
}
