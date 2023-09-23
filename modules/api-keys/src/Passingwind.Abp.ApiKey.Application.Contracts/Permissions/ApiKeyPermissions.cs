using Volo.Abp.Reflection;

namespace Passingwind.Abp.ApiKey.Permissions;

public class ApiKeyPermissions
{
    public const string GroupName = "ApiKey";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(ApiKeyPermissions));
    }
}
