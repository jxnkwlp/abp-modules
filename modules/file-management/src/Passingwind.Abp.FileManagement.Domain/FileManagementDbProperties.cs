namespace Passingwind.Abp.FileManagement;

public static class FileManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "Fm";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "FileManagement";
}
