namespace Passingwind.Abp.DynamicPermissionManagement;

public static class DynamicPermissionManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "DynamicPermissionManagement";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "DynamicPermissionManagement";
}
