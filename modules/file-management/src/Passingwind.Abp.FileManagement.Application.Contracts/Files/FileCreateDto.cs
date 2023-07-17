using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

public class FileCreateDto : ExtensibleObject
{
    public Guid? ParentId { get; set; }

    //public bool Override { get; set; }

    [Required]
    public IRemoteStreamContent File { get; set; } = null!;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var result in base.Validate(validationContext))
        {
            yield return result;
        }

        if (File == null || File?.ContentLength == 0)
        {
            yield return new ValidationResult("File should not be null", new[] { nameof(File) });
        }
    }
}
