using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.FileManagement.Files;

public class FileShareListRequestDto : PagedResultRequestDto
{
    public Guid? FileId { get; set; }
}