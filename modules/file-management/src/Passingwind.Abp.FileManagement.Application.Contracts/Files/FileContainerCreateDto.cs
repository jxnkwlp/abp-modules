using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerCreateDto : FileContainerCreateOrUpdateBasicDto
{
    [RegularExpression(@"^[\\u4e00-\\u9fa5A-Za-z0-9\\-\\_]*$")]
    [MaxLength(32)]
    public virtual string Name { get; set; } = null!;
}
