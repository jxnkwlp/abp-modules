using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileCreateByBytesDto : ExtensibleObject
{
    public Guid? ParentId { get; set; }
    [Required]
    public string FileName { get; set; } = null!;
    public string? MimeType { get; set; }
    [Required]
    public byte[] FileData { get; set; } = null!;
}
