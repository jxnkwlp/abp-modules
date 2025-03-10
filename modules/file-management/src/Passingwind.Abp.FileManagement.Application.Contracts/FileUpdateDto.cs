﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement;

public class FileUpdateDto : ExtensibleObject
{
    [Required]
    [MaxLength(128)]
    public string FileName { get; set; } = null!;

    public Dictionary<string, string?>? Tags { get; set; }
}
