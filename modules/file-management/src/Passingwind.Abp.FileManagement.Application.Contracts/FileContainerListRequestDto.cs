using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

public class FileContainerAdminListRequestDto : PagedResultRequestDto
{
    public string? Filter { get; set; }
}
