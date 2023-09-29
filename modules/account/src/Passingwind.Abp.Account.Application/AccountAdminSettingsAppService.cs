using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.Account.Settings;

namespace Passingwind.Abp.Account;

[Authorize(AccountPermissionNames.Settings.Default)]
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
        };
    }

    [Authorize(AccountPermissionNames.Settings.Update)]
    public virtual async Task UpdateAsync(AccountAdminSettingsDto input)
    {
        await AccountSettingsManager.SetGeneralSettingsAsync(ObjectMapper.Map<AccountGeneralSettingsDto, AccountGeneralSettings>(input.General));
        await AccountSettingsManager.SetCaptchaSettingsAsync(ObjectMapper.Map<AccountCaptchaSettingsDto, AccountCaptchaSettings>(input.Captcha));
        await AccountSettingsManager.SetRecaptchaSettingsAsync(ObjectMapper.Map<AccountRecaptchaSettingsDto, AccountRecaptchaSettings>(input.Recaptcha));
    }
}
