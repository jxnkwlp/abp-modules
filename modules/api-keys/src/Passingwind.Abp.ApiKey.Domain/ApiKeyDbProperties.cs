namespace Passingwind.Abp.ApiKey;

public static class ApiKeyDbProperties
{
    public static string DbTablePrefix { get; set; } = "Passingwind";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "ApiKey";
}
