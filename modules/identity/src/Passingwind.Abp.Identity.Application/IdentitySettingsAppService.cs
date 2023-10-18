using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Passingwind.Abp.Identity;

[Authorize(IdentityPermissionNamesV2.Settings.Identity)]
public class IdentitySettingsAppService : IdentityAppBaseService, IIdentitySettingsAppService
{
    protected IIdentitySettingsManager IdentitySettingsManager { get; }

    public IdentitySettingsAppService(IIdentitySettingsManager identitySettingsManager)
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

    public virtual async Task UpdateAsync(IdentitySettingsDto input)
    {
        await IdentitySettingsManager.SetSignInSettingsAsync(ObjectMapper.Map<IdentitySignInSettingsDto, IdentitySignInSettings>(input.SignIn));
        await IdentitySettingsManager.SetLockoutSettingsAsync(ObjectMapper.Map<IdentityLockoutSettingsDto, IdentityLockoutSettings>(input.Lockout));
        await IdentitySettingsManager.SetPasswordSettingsAsync(ObjectMapper.Map<IdentityPasswordSettingsDto, IdentityPasswordSettings>(input.Password));
        await IdentitySettingsManager.SetUserSettingsAsync(ObjectMapper.Map<IdentityUserSettingsDto, IdentityUserSettings>(input.User));
        await IdentitySettingsManager.SetTwofactorSettingsAsync(ObjectMapper.Map<IdentityTwofactorSettingsDto, IdentityTwofactorSettings>(input.Twofactor));
        await IdentitySettingsManager.SetOrganizationUnitSettingsAsync(ObjectMapper.Map<OrganizationUnitSettingsDto, OrganizationUnitSettings>(input.OrganizationUnit));
    }

    public virtual async Task<IdentityUserSettingsDto> GetUserAsync()
    {
        return ObjectMapper.Map<IdentityUserSettings, IdentityUserSettingsDto>(await IdentitySettingsManager.GetUserSettingsAsync());
    }

    public virtual async Task UpdateUserAsync(IdentityUserSettingsDto input)
    {
        await IdentitySettingsManager.SetUserSettingsAsync(ObjectMapper.Map<IdentityUserSettingsDto, IdentityUserSettings>(input));
    }

    public virtual async Task<IdentityPasswordSettingsDto> GetPasswordAsync()
    {
        return ObjectMapper.Map<IdentityPasswordSettings, IdentityPasswordSettingsDto>(await IdentitySettingsManager.GetPasswordSettingsAsync());
    }

    public virtual async Task UpdatePasswordAsync(IdentityPasswordSettingsDto input)
    {
        await IdentitySettingsManager.SetPasswordSettingsAsync(ObjectMapper.Map<IdentityPasswordSettingsDto, IdentityPasswordSettings>(input));
    }

    public virtual async Task<IdentityLockoutSettingsDto> GetLockoutAsync()
    {
        return ObjectMapper.Map<IdentityLockoutSettings, IdentityLockoutSettingsDto>(await IdentitySettingsManager.GetLockoutSettingsAsync());
    }

    public virtual async Task UpdateLockoutAsync(IdentityLockoutSettingsDto input)
    {
        await IdentitySettingsManager.SetLockoutSettingsAsync(ObjectMapper.Map<IdentityLockoutSettingsDto, IdentityLockoutSettings>(input));
    }

    public virtual async Task<IdentitySignInSettingsDto> GetSignInAsync()
    {
        return ObjectMapper.Map<IdentitySignInSettings, IdentitySignInSettingsDto>(await IdentitySettingsManager.GetSignInSettingsAsync());
    }

    public virtual async Task UpdateSignInAsync(IdentitySignInSettingsDto input)
    {
        await IdentitySettingsManager.SetSignInSettingsAsync(ObjectMapper.Map<IdentitySignInSettingsDto, IdentitySignInSettings>(input));
    }

    public virtual async Task<IdentityTwofactorSettingsDto> GetTwofactorAsync()
    {
        return ObjectMapper.Map<IdentityTwofactorSettings, IdentityTwofactorSettingsDto>(await IdentitySettingsManager.GetTwoFactorSettingsAsync());
    }

    public virtual async Task UpdateTwofactorAsync(IdentityTwofactorSettingsDto input)
    {
        await IdentitySettingsManager.SetTwofactorSettingsAsync(ObjectMapper.Map<IdentityTwofactorSettingsDto, IdentityTwofactorSettings>(input));
    }

    public virtual async Task<OrganizationUnitSettingsDto> GetOrganizationUnitAsync()
    {
        return ObjectMapper.Map<OrganizationUnitSettings, OrganizationUnitSettingsDto>(await IdentitySettingsManager.GetOrganizationUnitSettingsAsync());
    }

    public virtual async Task UpdateOrganizationUnitAsync(OrganizationUnitSettingsDto input)
    {
        await IdentitySettingsManager.SetOrganizationUnitSettingsAsync(ObjectMapper.Map<OrganizationUnitSettingsDto, OrganizationUnitSettings>(input));
    }
}
