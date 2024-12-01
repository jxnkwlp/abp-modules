using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.FileManagement;

public class FileDecompressRequestDto
{
    public bool Background { get; set; }
    public bool AutoDirectoryName { get; set; } = true;
    [MaxLength(64)]
    public string? DirectoryName { get; set; }
}
