using Passingwind.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Account;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(AccountPermissionNames.GroupName, L("Permission:AccountManagement"));

        var settings = context.GetGroup(SettingManagementPermissions.GroupName);

        settings.AddPermission(AccountPermissionNames.Settings.Account, L($"Permission:{AccountPermissionNames.Settings.Account}"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}
