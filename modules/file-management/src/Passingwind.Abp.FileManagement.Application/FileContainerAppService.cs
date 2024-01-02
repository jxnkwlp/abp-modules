using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.FileManagement.Files;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

[Authorize]
public class FileContainerAppService : FileManagementAppService, IFileContainerAppService
{
    protected IFileContainerManager FileContainerManager { get; }
    protected IFileContainerRepository FileContainerRepository { get; }

    public FileContainerAppService(IFileContainerManager fileContainerManager, IFileContainerRepository fileContainerRepository)
    {
        FileContainerManager = fileContainerManager;
        FileContainerRepository = fileContainerRepository;
    }

    public virtual async Task<ListResultDto<FileContainerBasicDto>> GetAllListAsync(FileContainerListRequestDto input)
    {
        var list = await FileContainerRepository.GetListAsync();

        // TODO

        return new ListResultDto<FileContainerBasicDto>(ObjectMapper.Map<List<FileContainer>, List<FileContainerBasicDto>>(list));
    }

    public virtual async Task<FileContainerBasicDto> GetByNameAsync(string name)
    {
        var entity = await FileContainerRepository.GetByNameAsync(name);

        return ObjectMapper.Map<FileContainer, FileContainerBasicDto>(entity);
    }

    public virtual async Task<FileContainerCanAccessResultDto> GetCanAccessAsync(string name)
    {
        _ = await FileContainerRepository.GetByNameAsync(name);

        // TODO

        return new FileContainerCanAccessResultDto { Result = false };
    }
}
