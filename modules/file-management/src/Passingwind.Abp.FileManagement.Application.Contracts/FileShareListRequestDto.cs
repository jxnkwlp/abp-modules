using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement;

public class FileShareListRequestDto : PagedResultRequestDto
{
    public Guid? FileId { get; set; }
}
