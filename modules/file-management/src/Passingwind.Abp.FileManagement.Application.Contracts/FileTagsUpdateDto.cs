using System.Collections.Generic;

namespace Passingwind.Abp.FileManagement;

public class FileTagsUpdateDto
{
    public Dictionary<string, string?> Tags { get; set; } = null!;
}
