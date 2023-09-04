namespace Passingwind.Abp.Account;

public static class AccountPermissionNames
{
    public const string GroupName = "Account";

    public static class Settings
    {
        public const string Default = GroupName + ".Settings";

        public const string Update = Default + ".Update";
    }
}
