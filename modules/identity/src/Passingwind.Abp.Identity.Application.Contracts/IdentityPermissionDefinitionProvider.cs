using Passingwind.Abp.Identity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Localization;

namespace Passingwind.Abp.Identity;

public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var identityGroup = context.GetGroup(IdentityPermissions.GroupName);

        var roleGroup = identityGroup.GetPermissionOrNull(IdentityPermissions.Roles.Default);
        roleGroup.AddChild(IdentityPermissionNamesV2.Roles.ManageClaims, L("Permission:ManageClaims"));
        //roleGroup.AddChild(IdentityPermissionNamesV2.Roles.ChangeHistories, L("Permission:ChangeHistories"));

        var userGroup = identityGroup.GetPermissionOrNull(IdentityPermissions.Users.Default);
        userGroup.AddChild(IdentityPermissionNamesV2.Users.ManageClaims, L("Permission:ManageClaims"));
        userGroup.AddChild(IdentityPermissionNamesV2.Users.ManageRoles, L("Permission:ManageRoles"));
        userGroup.AddChild(IdentityPermissionNamesV2.Users.ManageOrganizations, L("Permission:ManageOrganizations"));
        userGroup.AddChild(IdentityPermissionNamesV2.Users.Impersonation, L("Permission:Users.Impersonation"));
        //userGroup.AddChild(IdentityPermissionNamesV2.Users.Import, L("Permission:Import"));
        //userGroup.AddChild(IdentityPermissionNamesV2.Users.Export, L("Permission:Export"));
        //userGroup.AddChild(IdentityPermissionNamesV2.Users.ChangeHistories, L("Permission:ChangeHistories"));

        var ouGroup = identityGroup.AddPermission(IdentityPermissionNamesV2.OrganizationUnits.Default, L("Permission:OrganizationUnitManagement"));
        ouGroup.AddChild(IdentityPermissionNamesV2.OrganizationUnits.Manage, L("Permission:OrganizationUnitManagement.Manage"));
        ouGroup.AddChild(IdentityPermissionNamesV2.OrganizationUnits.Delete, L("Permission:Delete"));
        ouGroup.AddChild(IdentityPermissionNamesV2.OrganizationUnits.ManageUsers, L("Permission:ManageUsers"));
        ouGroup.AddChild(IdentityPermissionNamesV2.OrganizationUnits.ManageRoles, L("Permission:ManageRoles"));

        var claimTypesGroup = identityGroup.AddPermission(IdentityPermissionNamesV2.ClaimTypes.Default, L("Permission:ClaimTypeManagement"));
        claimTypesGroup.AddChild(IdentityPermissionNamesV2.ClaimTypes.Create, L("Permission:Create"));
        claimTypesGroup.AddChild(IdentityPermissionNamesV2.ClaimTypes.Update, L("Permission:Update"));
        claimTypesGroup.AddChild(IdentityPermissionNamesV2.ClaimTypes.Delete, L("Permission:Delete"));

        var securityLogsGroup = identityGroup.AddPermission(IdentityPermissionNamesV2.SecurityLogs.Default, L("Permission:SecurityLogManagement"));
        securityLogsGroup.AddChild(IdentityPermissionNamesV2.SecurityLogs.Delete, L("Permission:Delete"));

        var settingsGroup = identityGroup.AddPermission(IdentityPermissionNamesV2.Settings.Default, L("Permission:SettingManagement"));
        settingsGroup.AddChild(IdentityPermissionNamesV2.Settings.Update, L("Permission:Update"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResourceV2>(name);
    }
}
