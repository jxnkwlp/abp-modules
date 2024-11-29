using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Passingwind.Abp.AuditLogging.Cleanup;
using Passingwind.Abp.AuditLogging.Permissions;

namespace Passingwind.Abp.AuditLogging;

[Authorize(AuditLoggingPermissions.Settings.Default)]
public class AuditLogSettingAppService : AuditLoggingAppServiceBase, IAuditLogSettingAppService
{
    protected IAuditLogCleanupSettingManager SettingsManager { get; }

    public AuditLogSettingAppService(IAuditLogCleanupSettingManager settingsManager)
    {
        SettingsManager = settingsManager;
    }

    public virtual async Task<AuditLogCleanupSettingsDto> GetCleanupAsync()
    {
        var result = await SettingsManager.GetAsync();

        return new AuditLogCleanupSettingsDto
        {
            Behavior = result.Behavior,
            KeepDays = result.KeepDays,
        };
    }

    public virtual async Task UpdateCleanupAsync(AuditLogCleanupSettingsDto input)
    {
        await SettingsManager.UpdateAsync(new AuditLogCleanupSettings
        {
            Behavior = input.Behavior,
            KeepDays = input.KeepDays,
        });
    }
}
