using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileContainerAppService : IApplicationService
{
    Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerListRequestDto input);

    Task<FileContainerDto> GetAsync(Guid id);

    Task<FileContainerDto> GetByNameAsync(string name);

    Task<FileContainerDto> CreateAsync(FileContainerCreateOrUpdateDto input);

    Task<FileContainerDto> UpdateAsync(Guid id, FileContainerCreateOrUpdateDto input);

    Task DeleteAsync(Guid id);

}
