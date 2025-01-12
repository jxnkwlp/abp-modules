using Volo.Abp.Reflection;

namespace Passingwind.Abp.FileManagement.Permissions;

public class FileManagementPermissions
{
    public const string GroupName = "FileManagement";

    public static class FileContainers
    {
        public const string Default = GroupName + ".FileContainer";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Files
    {
        public const string Default = GroupName + ".File";
        public const string Update = GroupName + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Upload = GroupName + ".Upload";
        public const string Download = GroupName + ".Download";
    }

    public static class FileShares
    {
        public const string Default = GroupName + ".FileShares";
        public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileManagementPermissions));
    }
}
