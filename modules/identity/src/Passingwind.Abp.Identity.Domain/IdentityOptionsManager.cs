using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace Passingwind.Abp.Identity;

public class IdentityOptionsManager : AbpIdentityOptionsManager
{
    public IdentityOptionsManager(IOptionsFactory<IdentityOptions> factory, ISettingProvider settingProvider) : base(factory, settingProvider)
    {
    }

    protected override async Task OverrideOptionsAsync(string name, IdentityOptions options)
    {
        await base.OverrideOptionsAsync(name, options);

        options.User.RequireUniqueEmail = await SettingProvider.GetAsync<bool>(IdentitySettingNamesV2.User.RequireUniqueEmail);
    }
}
