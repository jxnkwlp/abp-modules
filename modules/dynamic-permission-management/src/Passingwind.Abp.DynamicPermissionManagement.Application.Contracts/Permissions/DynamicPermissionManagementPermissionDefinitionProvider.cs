using Passingwind.Abp.DynamicPermissionManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DynamicPermissionManagementPermissions.GroupName, L($"Permission:{DynamicPermissionManagementPermissions.GroupName}"));

        var permission = myGroup.AddPermission(DynamicPermissionManagementPermissions.PermissionDefinition.Default, L($"Permission:{DynamicPermissionManagementPermissions.PermissionDefinition.Default}"));

        permission.AddChild(DynamicPermissionManagementPermissions.PermissionDefinition.Manage, L($"Permission:{DynamicPermissionManagementPermissions.PermissionDefinition.Manage}"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DynamicPermissionManagementResource>(name);
    }
}
