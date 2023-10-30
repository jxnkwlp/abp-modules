using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Account.Settings;

namespace Passingwind.Abp.Account;

[Authorize(AccountPermissionNames.Settings.Account)]
public class AccountAdminSettingsAppService : AccountAppBaseService, IAccountAdminSettingsAppService
{
    protected IAccountSettingsManager AccountSettingsManager { get; }

    public AccountAdminSettingsAppService(IAccountSettingsManager accountSettingsManager)
    {
        AccountSettingsManager = accountSettingsManager;
    }

    public virtual async Task<AccountAdminSettingsDto> GetAsync()
    {
        return new AccountAdminSettingsDto()
        {
            General = ObjectMapper.Map<AccountGeneralSettings, AccountGeneralSettingsDto>(await AccountSettingsManager.GetGeneralSettingsAsync()),
            Captcha = ObjectMapper.Map<AccountCaptchaSettings, AccountCaptchaSettingsDto>(await AccountSettingsManager.GetCaptchaSettingsAsync()),
            Recaptcha = ObjectMapper.Map<AccountRecaptchaSettings, AccountRecaptchaSettingsDto>(await AccountSettingsManager.GetRecaptchaSettingsAsync()),
            ExternalLogin = ObjectMapper.Map<AccountExternalLoginSettings, AccountExternalLoginSettingsDto>(await AccountSettingsManager.GetExternalLoginSettingsAsync()),
        };
    }

    public virtual async Task UpdateAsync(AccountAdminSettingsDto input)
    {
        await AccountSettingsManager.SetGeneralSettingsAsync(ObjectMapper.Map<AccountGeneralSettingsDto, AccountGeneralSettings>(input.General));
        await AccountSettingsManager.SetCaptchaSettingsAsync(ObjectMapper.Map<AccountCaptchaSettingsDto, AccountCaptchaSettings>(input.Captcha));
        await AccountSettingsManager.SetRecaptchaSettingsAsync(ObjectMapper.Map<AccountRecaptchaSettingsDto, AccountRecaptchaSettings>(input.Recaptcha));
        await AccountSettingsManager.SetExternalLoginSettingsAsync(ObjectMapper.Map<AccountExternalLoginSettingsDto, AccountExternalLoginSettings>(input.ExternalLogin));
    }

    public virtual async Task<AccountGeneralSettingsDto> GetGeneralAsync()
    {
        return ObjectMapper.Map<AccountGeneralSettings, AccountGeneralSettingsDto>(await AccountSettingsManager.GetGeneralSettingsAsync());
    }

    public virtual async Task UpdateGeneralAsync(AccountGeneralSettingsDto input)
    {
        await AccountSettingsManager.SetGeneralSettingsAsync(ObjectMapper.Map<AccountGeneralSettingsDto, AccountGeneralSettings>(input));
    }

    public virtual async Task<AccountCaptchaSettingsDto> GetCaptchaAsync()
    {
        return ObjectMapper.Map<AccountCaptchaSettings, AccountCaptchaSettingsDto>(await AccountSettingsManager.GetCaptchaSettingsAsync());
    }

    public virtual async Task UpdateCaptchaAsync(AccountCaptchaSettingsDto input)
    {
        await AccountSettingsManager.SetCaptchaSettingsAsync(ObjectMapper.Map<AccountCaptchaSettingsDto, AccountCaptchaSettings>(input));
    }

    public virtual async Task<AccountRecaptchaSettingsDto> GetRecaptchaAsync()
    {
        return ObjectMapper.Map<AccountRecaptchaSettings, AccountRecaptchaSettingsDto>(await AccountSettingsManager.GetRecaptchaSettingsAsync());
    }

    public virtual async Task UpdateRecaptchaAsync(AccountRecaptchaSettingsDto input)
    {
        await AccountSettingsManager.SetRecaptchaSettingsAsync(ObjectMapper.Map<AccountRecaptchaSettingsDto, AccountRecaptchaSettings>(input));
    }

    public virtual async Task<AccountExternalLoginSettingsDto> GetExternalLoginAsync()
    {
        return ObjectMapper.Map<AccountExternalLoginSettings, AccountExternalLoginSettingsDto>(await AccountSettingsManager.GetExternalLoginSettingsAsync());
    }

    public virtual async Task UpdateExternalLoginAsync(AccountExternalLoginSettingsDto input)
    {
        await AccountSettingsManager.SetExternalLoginSettingsAsync(ObjectMapper.Map<AccountExternalLoginSettingsDto, AccountExternalLoginSettings>(input));
    }
}
