namespace Passingwind.Abp.AuditLogging.Settings;

public static class AuditLoggingSettingNames
{
    public const string GroupName = "AuditLogging";

    public static class Cleanup
    {
        private const string Default = GroupName + ".Cleanup";

        public const string Behavior = Default + ".Behavior";
        public const string KeepDays = Default + ".KeepDays";
        public const string SaveFileRootDir = Default + ".SaveFileRootDir";
    }
}
