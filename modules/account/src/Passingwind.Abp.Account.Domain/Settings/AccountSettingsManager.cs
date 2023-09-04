using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Account.Settings;

public class AccountSettingsManager : IAccountSettingsManager, IScopedDependency
{
    protected ISettingManager SettingManager { get; }

    public AccountSettingsManager(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    protected virtual async Task<string> GetSettingValueAsync(string name)
    {
        return await SettingManager.GetOrNullForCurrentTenantAsync(name);
    }

    protected virtual async Task<T> GetSettingValueAsync<T>(string name) where T : struct
    {
        string value = await GetSettingValueAsync(name);

        if (string.IsNullOrWhiteSpace(value))
            return default;

        if (typeof(T).IsEnum)
            return (T)Enum.Parse(typeof(T), value);

        return value.To<T>();
    }

    protected virtual async Task SetSettingValueAsync<T>(string name, T value) where T : struct
    {
        await SettingManager.SetForCurrentTenantAsync(name, value.ToString());
    }

    protected virtual async Task SetSettingValueAsync(string name, string? value)
    {
        await SettingManager.SetForCurrentTenantAsync(name, value);
    }

    public virtual async Task<AccountCaptchaSettings> GetCaptchaSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new AccountCaptchaSettings
        {
            EnableOnLogin = await GetSettingValueAsync<bool>(AccountSettingNames.Captcha.EnableOnLogin),
            EnableOnRegistration = await GetSettingValueAsync<bool>(AccountSettingNames.Captcha.EnableOnRegistration),
        };
    }

    public virtual async Task<AccountGeneralSettings> GetGeneralSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new AccountGeneralSettings
        {
            EnableLocalLogin = await GetSettingValueAsync<bool>(AccountSettingNames.General.EnableLocalLogin),
            IsSelfRegistrationEnabled = await GetSettingValueAsync<bool>(AccountSettingNames.General.IsSelfRegistrationEnabled),
        };
    }

    public virtual async Task<AccountRecaptchaSettings> GetRecaptchaSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new AccountRecaptchaSettings
        {
            Score = await GetSettingValueAsync<double>(AccountSettingNames.Recaptcha.Score),
            SiteKey = await GetSettingValueAsync(AccountSettingNames.Recaptcha.SiteKey),
            SiteSecret = await GetSettingValueAsync(AccountSettingNames.Recaptcha.SiteSecret),
            VerifyBaseUrl = await GetSettingValueAsync(AccountSettingNames.Recaptcha.VerifyBaseUrl),
            Version = await GetSettingValueAsync<int>(AccountSettingNames.Recaptcha.Version),
        };
    }

    public virtual async Task SetCaptchaSettingsAsync(AccountCaptchaSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(AccountSettingNames.Captcha.EnableOnLogin, settings.EnableOnLogin);
        await SetSettingValueAsync(AccountSettingNames.Captcha.EnableOnRegistration, settings.EnableOnRegistration);
    }

    public virtual async Task SetGeneralSettingsAsync(AccountGeneralSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(AccountSettingNames.General.IsSelfRegistrationEnabled, settings.IsSelfRegistrationEnabled);
        await SetSettingValueAsync(AccountSettingNames.General.EnableLocalLogin, settings.EnableLocalLogin);
    }

    public virtual async Task SetRecaptchaSettingsAsync(AccountRecaptchaSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(AccountSettingNames.Recaptcha.Score, settings.Score);
        await SetSettingValueAsync(AccountSettingNames.Recaptcha.SiteKey, settings.SiteKey);
        await SetSettingValueAsync(AccountSettingNames.Recaptcha.SiteSecret, settings.SiteSecret);
        await SetSettingValueAsync(AccountSettingNames.Recaptcha.VerifyBaseUrl, settings.VerifyBaseUrl);
        await SetSettingValueAsync(AccountSettingNames.Recaptcha.Version, settings.Version);
    }
}
