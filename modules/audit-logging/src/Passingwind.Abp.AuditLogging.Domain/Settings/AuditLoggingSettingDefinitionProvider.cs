using Volo.Abp.Settings;

namespace Passingwind.Abp.AuditLogging.Settings;

public class AuditLoggingSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        /* Define module settings here.
         * Use names from AuditLoggingSettings class.
         */

        context.Add(new SettingDefinition(AuditLoggingSettingNames.Cleanup.Behavior, nameof(AuditLogCleanupBehavior.None)));
        context.Add(new SettingDefinition(AuditLoggingSettingNames.Cleanup.KeepDays, "30"));
        context.Add(new SettingDefinition(AuditLoggingSettingNames.Cleanup.SaveFileRootDir, "./storage/audit-logs-backup"));
    }
}
