namespace Passingwind.Abp.IdentityClient;

public static class IdentityClientDbProperties
{
    public static string DbTablePrefix { get; set; } = "Passingwind";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "IdentityClient";
}
