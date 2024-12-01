namespace Passingwind.Abp.FileManagement;

public static class FileManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "App";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "FileManagement";
}
