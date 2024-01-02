using Microsoft.AspNetCore.Mvc;
using Passingwind.Abp.FileManagement.Files;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-containers")]
public class FileContainerController : FileManagementController, IFileContainerAppService
{
    private readonly IFileContainerAppService _service;

    public FileContainerController(IFileContainerAppService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    [HttpGet("all")]
    public virtual Task<ListResultDto<FileContainerBasicDto>> GetAllListAsync(FileContainerListRequestDto input)
    {
        return _service.GetAllListAsync(input);
    }

    /// <inheritdoc/>
    [HttpGet("by-name/{name}")]
    public virtual Task<FileContainerBasicDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }

    /// <inheritdoc/>
    [HttpGet("by-name/{name}/check-access")]
    public virtual Task<FileContainerCanAccessResultDto> GetCanAccessAsync(string name)
    {
        return _service.GetCanAccessAsync(name);
    }
}
