using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Content;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/files/compressions")]
public class FileCompressionController : FileManagementController, IFileCompressionAppService
{
    private readonly IFileCompressionAppService _service;

    public FileCompressionController(IFileCompressionAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpPost("{containerName}/compress/blob")]
    public virtual Task<IRemoteStreamContent> CompressAsync(string containerName, FileCompressToBlobRequestDto input)
    {
        return _service.CompressAsync(containerName, input);
    }

    /// <inheritdoc/>
    [HttpPost("{containerName}/compress")]
    public virtual Task<FileDto> CompressToFileAsync(string containerName, FileCompressRequestDto input)
    {
        return _service.CompressToFileAsync(containerName, input);
    }

    /// <inheritdoc/>
    [NonAction]
    public virtual Task<Stream?> CompressToStreamAsync(string containerName, FileCompressRequestDto input)
    {
        return _service.CompressToStreamAsync(containerName, input);
    }

    /// <inheritdoc/>
    [HttpPost("{containerName}/decompress/{id}")]
    public virtual Task<FileDecompressResultDto> DecompressAsync(string containerName, Guid id, FileDecompressRequestDto input)
    {
        return _service.DecompressAsync(containerName, id, input);
    }
}
