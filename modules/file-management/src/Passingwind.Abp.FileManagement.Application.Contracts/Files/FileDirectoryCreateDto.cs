using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.FileManagement.Files;

public class FileDirectoryCreateDto
{
    [Required]
    [MaxLength(128)]
    public string FileName { get; set; } = null!;

    public Guid? ParentId { get; set; }
}
