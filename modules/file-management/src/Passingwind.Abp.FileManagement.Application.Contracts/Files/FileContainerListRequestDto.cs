using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerListRequestDto : PagedResultRequestDto
{
    public string? Filter { get; set; }
}
