using Passingwind.Abp.DictionaryManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.DictionaryManagement.Permissions;

public class DictionaryManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DictionaryManagementPermissions.GroupName, L($"Permission:{DictionaryManagementPermissions.GroupName}"));

        var group = myGroup.AddPermission(DictionaryManagementPermissions.DictionaryGroup.Default, L($"Permission:{DictionaryManagementPermissions.DictionaryGroup.Default}"));
        group.AddChild(DictionaryManagementPermissions.DictionaryGroup.Create, L("Permission:Create"));
        group.AddChild(DictionaryManagementPermissions.DictionaryGroup.Update, L("Permission:Update"));
        group.AddChild(DictionaryManagementPermissions.DictionaryGroup.Delete, L("Permission:Delete"));

        var items = myGroup.AddPermission(DictionaryManagementPermissions.DictionaryItem.Default, L($"Permission:{DictionaryManagementPermissions.DictionaryItem.Default}"));
        items.AddChild(DictionaryManagementPermissions.DictionaryItem.Create, L("Permission:Create"));
        items.AddChild(DictionaryManagementPermissions.DictionaryItem.Update, L("Permission:Update"));
        items.AddChild(DictionaryManagementPermissions.DictionaryItem.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DictionaryManagementResource>(name);
    }
}
