using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement;

/// <summary>
///  The file container appservice for users
/// </summary>
public interface IFileContainerAppService : IApplicationService
{
    Task<ListResultDto<FileContainerBasicDto>> GetAllListAsync(FileContainerListRequestDto input);

    Task<FileContainerBasicDto> GetByNameAsync(string name);

    Task<FileContainerCanAccessResultDto> GetCanAccessAsync(string name);
}
