using Passingwind.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Passingwind.Abp.Identity.Settings;

public class IdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
           new SettingDefinition(
               IdentitySettingNamesV2.User.RequireUniqueEmail,
               true.ToString(),
               L("DisplayName:Abp.Identity.User.RequireUniqueEmail"))
        );

        context.Add(new SettingDefinition(IdentitySettingNamesV2.Twofactor.IsRememberBrowserEnabled, bool.TrueString, L("DisplayName:Identity.Twofactor.IsRememberBrowserEnabled"), isVisibleToClients: true));
        context.Add(new SettingDefinition(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour, nameof(IdentityTwofactoryBehaviour.Optional), L("DisplayName:Identity.Twofactor.TwoFactorBehaviour"), isVisibleToClients: true));
        context.Add(new SettingDefinition(IdentitySettingNamesV2.Twofactor.UsersCanChange, bool.TrueString, L("DisplayName:Identity.Twofactor.UsersCanChange"), isVisibleToClients: true));

        context.Add(new SettingDefinition(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled, bool.TrueString, L("DisplayName:Identity.Twofactor.AuthenticatorEnabled"), isVisibleToClients: true));
        context.Add(new SettingDefinition(IdentitySettingNamesV2.Twofactor.AuthenticatorIssuer, "MYAPP", L("DisplayName:Identity.Twofactor.AuthenticatorIssuer")));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResourceV2>(name);
    }
}
