using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

public class FileUpdateDto : ExtensibleObject
{
    [Required]
    [MaxLength(128)]
    public string FileName { get; set; } = null!;

}
