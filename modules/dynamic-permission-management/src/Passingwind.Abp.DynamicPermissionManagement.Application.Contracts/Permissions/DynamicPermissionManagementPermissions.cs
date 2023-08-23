using Volo.Abp.Reflection;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionManagementPermissions
{
    public const string GroupName = "DynamicPermissionManagement";

    public static class PermissionDefinition
    {
        public const string Default = GroupName + ".PermissionDefinition";
        public const string Manage = Default + ".Manage";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DynamicPermissionManagementPermissions));
    }
}
