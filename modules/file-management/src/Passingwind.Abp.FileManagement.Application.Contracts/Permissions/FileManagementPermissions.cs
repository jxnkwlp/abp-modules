using Volo.Abp.Reflection;

namespace Passingwind.Abp.FileManagement.Permissions;

public class FileManagementPermissions
{
    public const string GroupName = "FileManagement";

    public static class FileContainer
    {
        public const string Default = GroupName + ".FileContainer";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class File
    {
        public const string Default = GroupName + ".File";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileManagementPermissions));
    }
}
