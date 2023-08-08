namespace Passingwind.Abp.FileManagement;

public static class FileManagementErrorCodes
{
    //Add your business exception error codes here...

    public const string ContainerExist = "FileManagement:Error:ContainerExist";
    public const string ContainerFileQuantitiesMaximumSurpass = "FileManagement:Error:ContainerFileQuantitiesMaximumSurpass";
    public const string ContainerNotAllowForceDelete = "FileManagement:Error:ContainerNotAllowForceDelete";
    public const string FileExists = "FileManagement:Error:FileExists";
    public const string FileExtensionNotAllowed = "FileManagement:Error:FileExtensionNotAllowed";
    public const string FileLengthTooLarge = "FileManagement:Error:FileLengthTooLarge";
    public const string DirectoryExists = "FileManagement:Error:DirectoryExists";
    public const string DirectoryHasFiles = "FileManagement:Error:DirectoryHasFiles";
    public const string ShareFileNotExistsOrExpired = "FileManagement:Error:ShareFileNotExistsOrExpired";
}
