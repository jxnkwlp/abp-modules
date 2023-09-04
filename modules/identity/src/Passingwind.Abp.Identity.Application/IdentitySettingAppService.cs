using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissionNamesV2.Settings.Default)]
public class IdentitySettingAppService : IdentityAppBaseService, IIdentitySettingAppService
{
    protected IIdentitySettingsManager IdentitySettingsManager { get; }

    public IdentitySettingAppService(IIdentitySettingsManager identitySettingsManager)
    {
        IdentitySettingsManager = identitySettingsManager;
    }

    public virtual async Task<IdentitySettingsDto> GetAsync()
    {
        return new IdentitySettingsDto
        {
            SignIn = ObjectMapper.Map<IdentitySignInSettings, IdentitySignInSettingsDto>(await IdentitySettingsManager.GetSignInSettingsAsync()),
            Lockout = ObjectMapper.Map<IdentityLockoutSettings, IdentityLockoutSettingsDto>(await IdentitySettingsManager.GetLockoutSettingsAsync()),
            Password = ObjectMapper.Map<IdentityPasswordSettings, IdentityPasswordSettingsDto>(await IdentitySettingsManager.GetPasswordSettingsAsync()),
            User = ObjectMapper.Map<IdentityUserSettings, IdentityUserSettingsDto>(await IdentitySettingsManager.GetUserSettingsAsync()),
            Twofactor = ObjectMapper.Map<IdentityTwofactorSettings, IdentityTwofactorSettingsDto>(await IdentitySettingsManager.GetTwoFactorSettingsAsync()),
            OrganizationUnit = ObjectMapper.Map<OrganizationUnitSettings, OrganizationUnitSettingsDto>(await IdentitySettingsManager.GetOrganizationUnitSettingsAsync()),
        };
    }

    [Authorize(IdentityPermissionNamesV2.Settings.Update)]
    public virtual async Task UpdateAsync(IdentitySettingsDto input)
    {
        await IdentitySettingsManager.SetSignInSettingsAsync(ObjectMapper.Map<IdentitySignInSettingsDto, IdentitySignInSettings>(input.SignIn));
        await IdentitySettingsManager.SetLockoutSettingsAsync(ObjectMapper.Map<IdentityLockoutSettingsDto, IdentityLockoutSettings>(input.Lockout));
        await IdentitySettingsManager.SetPasswordSettingsAsync(ObjectMapper.Map<IdentityPasswordSettingsDto, IdentityPasswordSettings>(input.Password));
        await IdentitySettingsManager.SetUserSettingsAsync(ObjectMapper.Map<IdentityUserSettingsDto, IdentityUserSettings>(input.User));
        await IdentitySettingsManager.SetTwofactorSettingsAsync(ObjectMapper.Map<IdentityTwofactorSettingsDto, IdentityTwofactorSettings>(input.Twofactor));
        await IdentitySettingsManager.SetOrganizationUnitSettingsAsync(ObjectMapper.Map<OrganizationUnitSettingsDto, OrganizationUnitSettings>(input.OrganizationUnit));
    }
}
