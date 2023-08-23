namespace Passingwind.Abp.DictionaryManagement;

public static class DictionaryManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "Passingwind";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "DictionaryManagement";
}
