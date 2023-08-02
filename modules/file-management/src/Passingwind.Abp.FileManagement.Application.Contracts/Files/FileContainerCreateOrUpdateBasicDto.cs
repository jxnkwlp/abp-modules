using Volo.Abp.ObjectExtending;

namespace Passingwind.Abp.FileManagement.Files;

public class FileContainerCreateOrUpdateBasicDto : ExtensibleObject
{
    public virtual string? Description { get; set; }
    public virtual FileAccessMode? AccessMode { get; set; }
    public virtual long? MaximumEachFileSize { get; set; }
    public virtual int? MaximumFileQuantity { get; set; }
    public virtual FileOverrideBehavior? OverrideBehavior { get; set; }
    public virtual bool? AllowAnyFileExtension { get; set; }
    public virtual string? AllowedFileExtensions { get; set; }
    public virtual string? ProhibitedFileExtensions { get; set; }
    public virtual bool AutoDeleteBlob { get; set; }
}
