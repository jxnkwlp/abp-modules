using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  The file container management app service
/// </summary>
public interface IFileContainerAdminAppService : IApplicationService
{
    Task<ListResultDto<FileContainerDto>> GetAllListAsync();

    Task<PagedResultDto<FileContainerDto>> GetListAsync(FileContainerAdminListRequestDto input);

    Task<FileContainerDto> GetAsync(Guid id);

    Task<FileContainerDto> CreateAsync(FileContainerAdminCreateDto input);

    Task<FileContainerDto> UpdateAsync(Guid id, FileContainerAdminUpdateDto input);

    Task DeleteAsync(Guid id);
}
