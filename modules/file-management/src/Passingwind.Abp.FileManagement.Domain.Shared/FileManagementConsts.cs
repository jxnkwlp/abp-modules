namespace Passingwind.Abp.FileManagement;

public static class FileManagementConsts
{
    public static int MaxFileContainerNameLength { get; set; } = 64;
    public static int MaxFileContainerDescriptionLength { get; set; } = 256;

    public static int MaxFileItemFileNameLength { get; set; } = 256;
    public static int MaxFileItemMimeTypeLength { get; set; } = 128;
    public static int MaxFileItemBlobNameLength { get; set; } = 256;
    public static int MaxFileItemHashLength { get; set; } = 64;
    public static int MaxFileItemUniqueIdLength { get; set; } = 32;

    public static int MaxFileAccessTokenTokenLength { get; set; } = 256;

    public static int MaxFileTagValueLength { get; set; } = 64;
}
