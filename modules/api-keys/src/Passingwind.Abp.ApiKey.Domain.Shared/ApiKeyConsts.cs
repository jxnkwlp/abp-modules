namespace Passingwind.Abp.ApiKey;

public static class ApiKeyConsts
{
    public static int MaxApiKeyNameLength { get; set; } = 32;
    public static int MaxApiKeySecretLength { get; set; } = 128;
}
