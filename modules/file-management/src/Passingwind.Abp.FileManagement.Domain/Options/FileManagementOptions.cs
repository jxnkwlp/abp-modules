using Passingwind.Abp.FileManagement.Files;

namespace Passingwind.Abp.FileManagement.Options;

public class FileManagementOptions
{
    public string DefaultBlobContainer { get; set; } = "default";

    public bool FileContainerAsBlobContainer { get; set; } = true;

    public string BlobDirectorySeparator { get; set; } = "/";

    public string FileDownloadUrlFormat { get; set; } = "/api/files/download/{0}";

    /// <summary>
    ///  Default value for when create file container
    /// </summary>
    public FileAccessMode DefaultContainerAccessMode { get; set; } = FileAccessMode.Authorize;

    /// <summary>
    ///  Default value for when create file container
    /// </summary>
    public FileOverrideBehavior DefaultOverrideBehavior { get; set; } = FileOverrideBehavior.Rename;

    /// <summary>
    ///  Default value for when create file container
    /// </summary>
    public long DefaultMaximumFileSize { get; set; } = long.MaxValue;

    /// <summary>
    ///  Default value for when create file container
    /// </summary>
    public int DefaultContainerMaximumFileQuantity { get; set; } = int.MaxValue;

    /// <summary>
    ///  Default value for when create file container
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
