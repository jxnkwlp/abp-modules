using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.FileManagement;

public class FileDirectoryCreateDto
{
    [Required]
    [MaxLength(128)]
    public string FileName { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public bool Force { get; set; }
}
