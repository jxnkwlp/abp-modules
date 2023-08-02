using Passingwind.Abp.FileManagement.Files;

namespace Passingwind.Abp.FileManagement.Options;

/// <summary>
///  File management options
/// </summary>
public class FileManagementOptions
{
    /// <summary>
    ///  Default: 'default'
    /// </summary>
    public string DefaultBlobContainer { get; set; } = "default";

    /// <summary>
    ///  Default: true
    /// </summary>
    public bool FileContainerAsBlobContainer { get; set; } = true;

    /// <summary>
    ///  Default: '/'
    /// </summary>
    public string BlobDirectorySeparator { get; set; } = "/";

    /// <summary>
    ///  The file share url format. Default '/files/download/{0}'
    /// </summary>
    public string FileShareDownloadUrlFormat { get; set; } = "/files/download/{0}";

    /// <summary>
    ///  Default value for when create file container. Default <see cref="FileAccessMode.Authorized"/>
    /// </summary>
    public FileAccessMode DefaultContainerAccessMode { get; set; } = FileAccessMode.Authorized;

    /// <summary>
    ///  Default value for when create file container. Default <see cref="FileOverrideBehavior.Rename"/>
    /// </summary>
    public FileOverrideBehavior DefaultOverrideBehavior { get; set; } = FileOverrideBehavior.Rename;

    /// <summary>
    ///  Default value for when create file container. Default <see cref="long.MaxValue"/>
    /// </summary>
    public long DefaultMaximumFileSize { get; set; } = long.MaxValue;

    /// <summary>
    ///  Default value for when create file container. Default <see cref="int.MaxValue"/>
    /// </summary>
    public int DefaultContainerMaximumFileQuantity { get; set; } = int.MaxValue;

    /// <summary>
    ///  Default value for when create file container. Default '.txt,.png,.jpg,.jpeg,.bmp,.gif,.docx,.doc,.xlsx,.xls,.ppt,.pptx,.pdf,.zip,.rar,.7z'
    /// </summary>
    public string[]? DefaultAllowedFileExtensions { get; set; }

    /// <summary>
    ///  Default value for when create file container
    /// </summary>
    public string[]? DefaultProhibitedFileExtensions { get; set; }

    public FileManagementOptions()
    {
        DefaultAllowedFileExtensions = ".txt,.png,.jpg,.jpeg,.bmp,.gif,.docx,.doc,.xlsx,.xls,.ppt,.pptx,.pdf,.zip,.rar,.7z".Split(',');
    }
}
