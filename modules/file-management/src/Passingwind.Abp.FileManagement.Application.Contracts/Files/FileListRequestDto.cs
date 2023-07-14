using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FileListRequestDto : PagedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? ParentId { get; set; }
    public bool OnlyDirectory { get; set; }
}
