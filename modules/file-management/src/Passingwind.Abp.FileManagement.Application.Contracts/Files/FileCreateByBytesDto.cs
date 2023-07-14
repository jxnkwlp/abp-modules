using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

public class FileCreateByBytesDto : ExtensibleObject
{
    public Guid? ParentId { get; set; }
    [Required]
    [MaxLength(128)]
    public string FileName { get; set; } = null!;
    [MaxLength(32)]
    public string? MimeType { get; set; }
    [Required]
    public byte[] FileData { get; set; } = null!;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }

        if (FileData == null)
        {
            yield return new ValidationResult("FileData should not be null", new[] { nameof(FileData) });
        }
    }
}