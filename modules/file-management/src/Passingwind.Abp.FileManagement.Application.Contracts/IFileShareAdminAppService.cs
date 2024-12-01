using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  The file share management app service
/// </summary>
public interface IFileShareAdminAppService : IApplicationService
{
    Task<PagedResultDto<FileShareResultDto>> GetListAsync(FileShareAdminListRequestDto input);

    Task<FileShareResultDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);
}
