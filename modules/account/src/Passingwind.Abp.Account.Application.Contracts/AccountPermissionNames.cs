using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Account;

public static class AccountPermissionNames
{
    public const string GroupName = "Account";

    public static class Settings
    {
        public const string GroupName = SettingManagementPermissions.GroupName;

        public const string Account = GroupName + ".Account";
    }
}
