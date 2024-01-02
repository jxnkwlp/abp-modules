using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Passingwind.Abp.FileManagement.Files;

public interface IFileContainerAppService : IApplicationService
{
    Task<ListResultDto<FileContainerBasicDto>> GetAllListAsync(FileContainerListRequestDto input);

    Task<FileContainerBasicDto> GetByNameAsync(string name);

    Task<FileContainerCanAccessResultDto> GetCanAccessAsync(string name);
}
