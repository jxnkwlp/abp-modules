using Volo.Abp.Identity;
using Volo.Abp.Reflection;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Identity;

public class IdentityPermissionNamesV2
{
    public const string GroupName = IdentityPermissions.GroupName;

    public static class Users
    {
        public const string Default = GroupName + ".Users";

        public const string ChangeHistories = Default + ".ChangeHistories";

        public const string ManageClaims = Default + ".ManageClaims";
        public const string ManageRoles = Default + ".ManageRoles";
        public const string ManageOrganizations = Default + ".ManageOrganizations";

        public const string Impersonation = Default + ".Impersonation";
        public const string Import = Default + ".Import";
        public const string Export = Default + ".Export";
    }

    public static class Roles
    {
        public const string Default = GroupName + ".Roles";

        public const string ManageClaims = Default + ".ManageClaims";
        public const string ChangeHistories = Default + ".ChangeHistories";
    }

    public static class OrganizationUnits
    {
        public const string Default = GroupName + ".OrganizationUnits";

        public const string Manage = Default + ".Manage";
        public const string Delete = Default + ".Delete";

        public const string ManageUsers = Default + ".ManageUsers";
        public const string ManageRoles = Default + ".ManageRoles";
    }

    public static class ClaimTypes
    {
        public const string Default = GroupName + ".ClaimTypes";

        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class SecurityLogs
    {
        public const string Default = GroupName + ".SecurityLogs";

        public const string Delete = Default + ".Delete";
    }

    public static class Settings
    {
        public const string GroupName = SettingManagementPermissions.GroupName;

        public const string Identity = GroupName + ".Identity";
        //public const string Update = Identity + ".Update";
    }

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissionNamesV2));
    }
}
