namespace Passingwind.Abp.IdentityClientManagement;

public static class IdentityClientManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "Passingwind";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "IdentityClientManagement";
}
