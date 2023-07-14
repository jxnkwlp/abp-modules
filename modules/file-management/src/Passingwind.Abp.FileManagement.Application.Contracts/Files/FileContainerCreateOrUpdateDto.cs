using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerCreateOrUpdateDto : ExtensibleObject
{
    [RegularExpression(@"^[\\u4e00-\\u9fa5A-Za-z0-9\\-\\_]*$")]
    public virtual string Name { get; set; } = null!;
    public virtual string? Description { get; set; }
    public virtual FileAccessMode AccessMode { get; set; }
    public virtual long? MaximumEachFileSize { get; set; }
    public virtual int? MaximumFileQuantity { get; set; }
    public virtual FileOverrideBehavior? OverrideBehavior { get; set; }
    public virtual bool AllowAnyFileExtension { get; set; }
    public virtual string? AllowedFileExtensions { get; set; }
    public virtual string? ProhibitedFileExtensions { get; set; }
}
