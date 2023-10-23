using Volo.Abp.Reflection;

namespace Passingwind.Abp.IdentityClient.Permissions;

public class IdentityClientPermissions
{
    public const string GroupName = "IdentityClient";

    public static class IdentityClient
    {
        public const string Default = GroupName + ".IdentityClient";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityClientPermissions));
    }
}
