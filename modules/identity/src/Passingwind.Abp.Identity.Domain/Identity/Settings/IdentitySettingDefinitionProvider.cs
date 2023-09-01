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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResourceV2>(name);
    }
}
