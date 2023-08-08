using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FilePagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? ParentId { get; set; }
    public bool? IsDirectory { get; set; }
}
