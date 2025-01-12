using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement;

public class FileCreateByStreamDto : ExtensibleObject
{
    public Guid? ParentId { get; set; }
    [MaxLength(128)]
    public string FileName { get; set; } = null!;
    [MaxLength(32)]
    public string? MimeType { get; set; }
    [Required]
    public Stream FileStream { get; set; } = null!;

    public bool Override { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }

        if (FileStream == null)
        {
            yield return new ValidationResult("FileStream should not be null", new[] { nameof(FileStream) });
        }
    }
}
