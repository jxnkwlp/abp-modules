using Passingwind.Abp.IdentityClientManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.IdentityClientManagement.Permissions;

public class IdentityClientManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IdentityClientManagementPermissions.GroupName, L($"Permission:{IdentityClientManagementPermissions.GroupName}"));

        var identityClient = myGroup.AddPermission(IdentityClientManagementPermissions.IdentityClient.Default, L($"Permission:{IdentityClientManagementPermissions.IdentityClient.Default}"));
        identityClient.AddChild(IdentityClientManagementPermissions.IdentityClient.Create, L("Permission:Create"));
        identityClient.AddChild(IdentityClientManagementPermissions.IdentityClient.Update, L("Permission:Update"));
        identityClient.AddChild(IdentityClientManagementPermissions.IdentityClient.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityClientManagementResource>(name);
    }
}
