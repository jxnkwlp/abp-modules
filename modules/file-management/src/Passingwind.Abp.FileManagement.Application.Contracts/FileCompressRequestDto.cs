using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.FileManagement;

public class FileCompressRequestDto
{
    [Required]
    public Guid[] Ids { get; set; } = null!;

    [Required]
    [MaxLength(32)]
    public string FileName { get; set; } = null!;
}
