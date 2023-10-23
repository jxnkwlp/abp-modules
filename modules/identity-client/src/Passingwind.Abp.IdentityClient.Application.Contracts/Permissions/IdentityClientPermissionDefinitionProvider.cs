using Passingwind.Abp.IdentityClient.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.IdentityClient.Permissions;

public class IdentityClientPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IdentityClientPermissions.GroupName, L($"Permission:{IdentityClientPermissions.GroupName}"));

        var identityClient = myGroup.AddPermission(IdentityClientPermissions.IdentityClient.Default, L($"Permission:{IdentityClientPermissions.IdentityClient.Default}"));
        identityClient.AddChild(IdentityClientPermissions.IdentityClient.Create, L("Permission:Create"));
        identityClient.AddChild(IdentityClientPermissions.IdentityClient.Update, L("Permission:Update"));
        identityClient.AddChild(IdentityClientPermissions.IdentityClient.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityClientResource>(name);
    }
}
