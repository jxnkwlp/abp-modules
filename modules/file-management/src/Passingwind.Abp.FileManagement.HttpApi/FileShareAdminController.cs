using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Route("api/file-management/shares")]
public class FileShareAdminController : FileManagementController, IFileShareAdminAppService
{
    private readonly IFileShareAdminAppService _service;

    public FileShareAdminController(IFileShareAdminAppService service)
    {
        _service = service;
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public Task<FileShareResultDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<FileShareResultDto>> GetListAsync(FileShareAdminListRequestDto input)
    {
        return _service.GetListAsync(input);
    }
}
