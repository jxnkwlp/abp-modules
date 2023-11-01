using System;
using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.Identity.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;

namespace Passingwind.Abp.Identity;

public class IdentitySettingsManager : IIdentitySettingsManager, ITransientDependency
{
    protected ISettingManager SettingManager { get; }

    public IdentitySettingsManager(ISettingManager settingManager)
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

    protected virtual async Task SetSettingValueAsync(string name, string? value)
    {
        await SettingManager.SetForCurrentTenantAsync(name, value);
    }

    protected virtual async Task SetSettingValueAsync<T>(string name, T value) where T : struct
    {
        await SettingManager.SetForCurrentTenantAsync(name, value.ToString());
    }

    public virtual async Task<IdentityUserSettings> GetUserSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new IdentityUserSettings()
        {
            IsEmailUpdateEnabled = await GetSettingValueAsync<bool>(IdentitySettingNames.User.IsEmailUpdateEnabled),
            IsUserNameUpdateEnabled = await GetSettingValueAsync<bool>(IdentitySettingNames.User.IsUserNameUpdateEnabled),
            RequireUniqueEmail = await GetSettingValueAsync<bool>(IdentitySettingNamesV2.User.RequireUniqueEmail),
        };
    }

    public virtual async Task<IdentityPasswordSettings> GetPasswordSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new IdentityPasswordSettings()
        {
            ForceUsersToPeriodicallyChangePassword = (await GetSettingValueAsync<bool>(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword)),
            PasswordChangePeriodDays = (await GetSettingValueAsync<int>(IdentitySettingNames.Password.PasswordChangePeriodDays)),
            RequireDigit = (await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireDigit)),
            RequiredLength = (await GetSettingValueAsync<int>(IdentitySettingNames.Password.RequiredLength)),
            RequiredUniqueChars = (await GetSettingValueAsync<int>(IdentitySettingNames.Password.RequiredUniqueChars)),
            RequireLowercase = (await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireLowercase)),
            RequireNonAlphanumeric = (await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireNonAlphanumeric)),
            RequireUppercase = (await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireUppercase)),
        };
    }

    public virtual async Task<IdentityLockoutSettings> GetLockoutSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new IdentityLockoutSettings
        {
            AllowedForNewUsers = (await GetSettingValueAsync<bool>(IdentitySettingNames.Lockout.AllowedForNewUsers)),
            LockoutDuration = (await GetSettingValueAsync<int>(IdentitySettingNames.Lockout.LockoutDuration)),
            MaxFailedAccessAttempts = (await GetSettingValueAsync<int>(IdentitySettingNames.Lockout.MaxFailedAccessAttempts)),
        };
    }

    public virtual async Task<IdentitySignInSettings> GetSignInSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new IdentitySignInSettings
        {
            EnablePhoneNumberConfirmation = (await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation)),
            RequireConfirmedEmail = (await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.RequireConfirmedEmail)),
            RequireConfirmedPhoneNumber = (await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber)),
        };
    }

    public virtual async Task<OrganizationUnitSettings> GetOrganizationUnitSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new OrganizationUnitSettings
        {
            MaxUserMembershipCount = (await GetSettingValueAsync<int>(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount)),
        };
    }

    public virtual async Task SetUserSettingsAsync(IdentityUserSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled, settings.IsEmailUpdateEnabled);
        await SetSettingValueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled, settings.IsUserNameUpdateEnabled);
        await SetSettingValueAsync(IdentitySettingNamesV2.User.RequireUniqueEmail, settings.RequireUniqueEmail);
    }

    public virtual async Task SetPasswordSettingsAsync(IdentityPasswordSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNames.Password.RequiredLength, settings.RequiredLength);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequiredUniqueChars, settings.RequiredUniqueChars);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, settings.RequireNonAlphanumeric);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireLowercase, settings.RequireLowercase);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireUppercase, settings.RequireUppercase);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireDigit, settings.RequireDigit);
        await SetSettingValueAsync(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, settings.ForceUsersToPeriodicallyChangePassword);
        await SetSettingValueAsync(IdentitySettingNames.Password.PasswordChangePeriodDays, settings.PasswordChangePeriodDays);
    }

    public virtual async Task SetLockoutSettingsAsync(IdentityLockoutSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, settings.AllowedForNewUsers);
        await SetSettingValueAsync(IdentitySettingNames.Lockout.LockoutDuration, settings.LockoutDuration);
        await SetSettingValueAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, settings.MaxFailedAccessAttempts);
    }

    public virtual async Task SetSignInSettingsAsync(IdentitySignInSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, settings.RequireConfirmedEmail);
        await SetSettingValueAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, settings.EnablePhoneNumberConfirmation);
        await SetSettingValueAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, settings.RequireConfirmedPhoneNumber);
    }

    public virtual async Task SetOrganizationUnitSettingsAsync(OrganizationUnitSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount, settings.MaxUserMembershipCount);
    }

    public async Task<IdentityTwofactorSettings> GetTwoFactorSettingsAsync(CancellationToken cancellationToken = default)
    {
        return new IdentityTwofactorSettings
        {
            IsRememberBrowserEnabled = await GetSettingValueAsync<bool>(IdentitySettingNamesV2.Twofactor.IsRememberBrowserEnabled),
            TwoFactorBehaviour = await GetSettingValueAsync<IdentityTwofactoryBehaviour>(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour),
            UsersCanChange = await GetSettingValueAsync<bool>(IdentitySettingNamesV2.Twofactor.UsersCanChange),

            AuthenticatorIssuer = await GetSettingValueAsync(IdentitySettingNamesV2.Twofactor.AuthenticatorIssuer),
            AuthenticatorEnabled = await GetSettingValueAsync<bool>(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled),
        };
    }

    public async Task SetTwofactorSettingsAsync(IdentityTwofactorSettings settings, CancellationToken cancellationToken = default)
    {
        await SetSettingValueAsync(IdentitySettingNamesV2.Twofactor.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled);
        await SetSettingValueAsync(IdentitySettingNamesV2.Twofactor.TwoFactorBehaviour, settings.TwoFactorBehaviour);
        await SetSettingValueAsync(IdentitySettingNamesV2.Twofactor.UsersCanChange, settings.UsersCanChange);

        await SetSettingValueAsync(IdentitySettingNamesV2.Twofactor.AuthenticatorEnabled, settings.AuthenticatorEnabled);
        await SetSettingValueAsync(IdentitySettingNamesV2.Twofactor.AuthenticatorIssuer, settings.AuthenticatorIssuer);
    }
}
