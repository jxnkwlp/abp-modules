using Volo.Abp.Reflection;

namespace Passingwind.Abp.DictionaryManagement.Permissions;

public class DictionaryManagementPermissions
{
    public const string GroupName = "DictionaryManagement";

    public static class DictionaryGroup
    {
        public const string Default = GroupName + ".DictionaryGroups";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class DictionaryItem
    {
        public const string Default = GroupName + ".DictionaryItems";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DictionaryManagementPermissions));
    }
}
