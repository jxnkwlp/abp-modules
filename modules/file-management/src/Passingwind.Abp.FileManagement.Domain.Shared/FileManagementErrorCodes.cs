namespace Passingwind.Abp.FileManagement;

public static class FileManagementErrorCodes
{
    //Add your business exception error codes here...

    public const string FileContainerExist = "FileManagement:ContainerExist";
    public const string FileExtensionNotAllowed = "FileManagement:FileExtensionNotAllowed";
    public const string FileLengthTooLarge = "FileManagement:FileLengthTooLarge";
    public const string ContainerFileQuantitiesMaximumSurpass = "FileManagement:ContainerFileQuantitiesMaximumSurpass";
    public const string FileExists = "FileManagement:FileExists";

    public const string DownloadNotExistsOrExpired = "FileManagement:DownloadNotExistsOrExpired";
}
