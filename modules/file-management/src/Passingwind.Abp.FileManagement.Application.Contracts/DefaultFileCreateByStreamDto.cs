using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement;

public class DefaultFileCreateByStreamDto : ExtensibleObject
{
    public Guid? ParentId { get; set; }
    [Required]
    public string FileName { get; set; } = null!;
    public string? MimeType { get; set; }
    [Required]
    public Stream FileStream { get; set; } = null!;
}
