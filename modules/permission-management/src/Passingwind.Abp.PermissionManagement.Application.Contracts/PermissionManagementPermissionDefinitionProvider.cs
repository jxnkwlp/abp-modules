using Passingwind.Abp.PermissionManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.PermissionManagement;

public class PermissionManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PermissionManagementPermissions.GroupName, L($"Permission:{PermissionManagementPermissions.GroupName}"));

        var dynamicPermissions = myGroup.AddPermission(PermissionManagementPermissions.DynamicPermissions.Default, L($"Permission:{PermissionManagementPermissions.DynamicPermissions.Default}"));
        dynamicPermissions.AddChild(PermissionManagementPermissions.DynamicPermissions.Manage, L($"Permission:{PermissionManagementPermissions.DynamicPermissions.Manage}"));

        var permissions = myGroup.AddPermission(PermissionManagementPermissions.Permissions.Default, L($"Permission:{PermissionManagementPermissions.Permissions.Default}"));
        permissions.AddChild(PermissionManagementPermissions.Permissions.Manage, L($"Permission:{PermissionManagementPermissions.Permissions.Manage}"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PermissionManagementResource>(name);
    }
}
