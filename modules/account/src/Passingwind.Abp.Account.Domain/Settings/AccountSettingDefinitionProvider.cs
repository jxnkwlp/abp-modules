using Passingwind.Abp.Account.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Passingwind.Abp.Account.Settings;

public class AccountSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //context.Add(
        //    new SettingDefinition(
        //        AccountSettingNames.General.IsSelfRegistrationEnabled,
        //        "true",
        //        L("DisplayName:Abp.Account.IsSelfRegistrationEnabled"),
        //        L("Description:Abp.Account.IsSelfRegistrationEnabled"), isVisibleToClients: true)
        //);

        //context.Add(
        //    new SettingDefinition(
        //        AccountSettingNames.General.EnableLocalLogin,
        //        "true",
        //        L("DisplayName:Abp.Account.EnableLocalLogin"),
        //        L("Description:Abp.Account.EnableLocalLogin"), isVisibleToClients: true)
        //);

        context.Add(new SettingDefinition(AccountSettingNames.Captcha.EnableOnLogin, "false", L("DisplayName:Account.Captcha.EnableOnLogin"), isVisibleToClients: true));
        context.Add(new SettingDefinition(AccountSettingNames.Captcha.EnableOnRegistration, "false", L("DisplayName:Account.Captcha.EnableOnRegistration"), isVisibleToClients: true));

        context.Add(new SettingDefinition(AccountSettingNames.Recaptcha.Score, "0.5", L("DisplayName:Account.Recaptcha.Score"), isVisibleToClients: true));
        context.Add(new SettingDefinition(AccountSettingNames.Recaptcha.SiteKey, null, L("DisplayName:Account.Recaptcha.SiteKey"), isVisibleToClients: true));
        context.Add(new SettingDefinition(AccountSettingNames.Recaptcha.SiteSecret, null, L("DisplayName:Account.Recaptcha.SiteSecret")));
        context.Add(new SettingDefinition(AccountSettingNames.Recaptcha.VerifyBaseUrl, null, L("DisplayName:Account.Recaptcha.VerifyBaseUrl"), isVisibleToClients: true));
        context.Add(new SettingDefinition(AccountSettingNames.Recaptcha.Version, "3", L("DisplayName:Account.Recaptcha.Version"), isVisibleToClients: true));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}
