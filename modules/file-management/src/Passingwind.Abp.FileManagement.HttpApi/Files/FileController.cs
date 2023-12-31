﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement.Files;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-management")]
public class FileController : FileManagementController, IFileAppService
{
    private readonly IFileAppService _service;

    public FileController(IFileAppService service)
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

    [NonAction]
    public virtual Task<Stream?> GeBlobStreamAsync(string containerName, Guid id)
    {
        return _service.GeBlobStreamAsync(containerName, id);
    }

    [NonAction]
    public virtual Task<byte[]> GetBlobBytesAsync(string containerName, Guid id)
    {
        return _service.GetBlobBytesAsync(containerName, id);
    }

    [HttpPost("{containerName}")]
    [Consumes("multipart/form-data")]
    public virtual Task<FileDto> CreateAsync(string containerName, [FromForm] FileCreateDto input)
    {
        return _service.CreateAsync(containerName, input);
    }

    [NonAction]
    public virtual Task<FileDto> CreateByStreamAsync(string containerName, FileCreateByStreamDto input)
    {
        return _service.CreateByStreamAsync(containerName, input);
    }

    [NonAction]
    public virtual Task<FileDto> CreateByBytesAsync(string containerName, FileCreateByBytesDto input)
    {
        return _service.CreateByBytesAsync(containerName, input);
    }

    [HttpPost("{containerName}/{id}/move")]
    public virtual Task<FileDto> MoveAsync(string containerName, Guid id, FileMoveRequestDto input)
    {
        return _service.MoveAsync(containerName, id, input);
    }

    [HttpDelete("{containerName}/{id}")]
    public virtual Task DeleteAsync(string containerName, Guid id)
    {
        return _service.DeleteAsync(containerName, id);
    }

    [HttpPut("{containerName}/{id}")]
    public virtual Task<FileDto> UpdateAsync(string containerName, Guid id, FileUpdateDto input)
    {
        return _service.UpdateAsync(containerName, id, input);
    }

    [HttpPost("{containerName}/directory")]
    public virtual Task<FileDto> CreateDirectoryAsync(string containerName, FileDirectoryCreateDto input)
    {
        return _service.CreateDirectoryAsync(containerName, input);
    }
}
