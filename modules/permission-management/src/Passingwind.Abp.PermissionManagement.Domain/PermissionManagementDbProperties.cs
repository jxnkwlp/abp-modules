namespace Passingwind.Abp.PermissionManagement;

public static class PermissionManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "Passingwind";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "PermissionManagement";
}
