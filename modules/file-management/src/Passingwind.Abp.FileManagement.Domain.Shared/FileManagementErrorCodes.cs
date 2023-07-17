namespace Passingwind.Abp.FileManagement;

public static class FileManagementErrorCodes
{
    //Add your business exception error codes here...

    public const string FileContainerExist = "FileManagement:Error:ContainerExist";
    public const string FileExtensionNotAllowed = "FileManagement:Error:FileExtensionNotAllowed";
    public const string FileLengthTooLarge = "FileManagement:Error:FileLengthTooLarge";
    public const string ContainerFileQuantitiesMaximumSurpass = "FileManagement:Error:ContainerFileQuantitiesMaximumSurpass";
    public const string FileExists = "FileManagement:Error:FileExists";
    public const string DirectoryExists = "FileManagement:Error:DirectoryExists";
    public const string DownloadNotExistsOrExpired = "FileManagement:Error:DownloadNotExistsOrExpired";
}
