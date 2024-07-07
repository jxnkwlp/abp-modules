using Volo.Abp.Reflection;

namespace Passingwind.Abp.PermissionManagement;

public class PermissionManagementPermissions
{
    public const string GroupName = "PermissionManagement";

    public static class Permissions
    {
        public const string Default = GroupName + ".Permissions";
        public const string Manage = Default + ".Manage";
    }

    public static class DynamicPermissions
    {
        public const string Default = GroupName + ".DynamicPermissions";
        public const string Manage = Default + ".Manage";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(PermissionManagementPermissions));
    }
}
