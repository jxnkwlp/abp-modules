namespace Passingwind.Abp.Identity;

public static class IdentityDbProperties
{
    public static string DbTablePrefix { get; set; } = "Identity";

    public static string? DbSchema { get; set; }

    public const string ConnectionStringName = "Identity";
}
