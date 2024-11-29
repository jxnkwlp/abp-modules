using Volo.Abp.Reflection;

namespace Passingwind.Abp.AuditLogging.Permissions;

public class AuditLoggingPermissions
{
    public const string GroupName = "AuditLogging";

    public static class AuditLogs
    {
        public const string Default = GroupName + ".AuditLogs";

        public const string Delete = Default + ".Delete";
    }

    public static class Settings
    {
        public const string Default = GroupName + ".Settings";

        public const string Cleanup = Default + ".Cleanup";
    }

    public static class EntityChange
    {
        public const string Default = GroupName + ".EntityChanges";
        //public const string Delete = Default + ".Delete";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AuditLoggingPermissions));
    }
}
