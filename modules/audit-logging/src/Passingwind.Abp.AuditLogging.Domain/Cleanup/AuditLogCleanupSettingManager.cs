using System;
using System.Threading;
using System.Threading.Tasks;
using Passingwind.Abp.AuditLogging.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Passingwind.Abp.AuditLogging.Cleanup;

public class AuditLogCleanupSettingManager : IAuditLogCleanupSettingManager, IScopedDependency
{
    protected ISettingProvider SettingProvider { get; }
    protected ISettingManager SettingManager { get; }

    public AuditLogCleanupSettingManager(ISettingProvider settingProvider, ISettingManager settingManager)
    {
        SettingProvider = settingProvider;
        SettingManager = settingManager;
    }

    public virtual async Task<AuditLogCleanupSettings> GetAsync(CancellationToken cancellationToken = default)
    {
        return new AuditLogCleanupSettings
        {
            Behavior = await GetValue<AuditLogCleanupBehavior>(AuditLoggingSettingNames.Cleanup.Behavior, cancellationToken: cancellationToken),
            KeepDays = await GetValue<int>(AuditLoggingSettingNames.Cleanup.KeepDays, cancellationToken: cancellationToken),
        };
    }

    public virtual async Task UpdateAsync(AuditLogCleanupSettings settings, CancellationToken cancellationToken = default)
    {
        await SettingManager.SetGlobalAsync(AuditLoggingSettingNames.Cleanup.KeepDays, settings.KeepDays.ToString());
        await SettingManager.SetGlobalAsync(AuditLoggingSettingNames.Cleanup.Behavior, settings.Behavior.ToString());
    }

    protected virtual async Task<T> GetValue<T>(string key, T defaultValue = default, CancellationToken cancellationToken = default) where T : struct
    {
        var value = await SettingProvider.GetOrNullAsync(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (typeof(T).IsEnum)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        return value!.To<T>();
    }
}
