using Passingwind.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.Account;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(AccountPermissionNames.GroupName, L("Permission:AccountManagement"));

        var settingsGroup = group.AddPermission(AccountPermissionNames.Settings.Default, L("Permission:SettingManagement"));
        settingsGroup.AddChild(AccountPermissionNames.Settings.Update, L("Permission:Update"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}
