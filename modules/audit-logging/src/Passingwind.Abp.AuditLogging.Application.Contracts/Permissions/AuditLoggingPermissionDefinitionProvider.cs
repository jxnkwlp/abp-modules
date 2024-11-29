using Passingwind.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Passingwind.Abp.AuditLogging.Permissions;

public class AuditLoggingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1201:Use method chaining.", Justification = "<Pending>")]
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AuditLoggingPermissions.GroupName, L("Permission:AuditLogging"));

        var settings = myGroup.AddPermission(AuditLoggingPermissions.Settings.Default, L("Permission:Settings"));
        settings.AddChild(AuditLoggingPermissions.Settings.Cleanup, L("Permission:AuditLogging.Cleanup"));

        var auditLogs = myGroup.AddPermission(AuditLoggingPermissions.AuditLogs.Default, L("Permission:AuditLogging.AuditLogs"));
        auditLogs.AddChild(AuditLoggingPermissions.AuditLogs.Delete, L("Permission:Delete"));

        var entityChanges = myGroup.AddPermission(AuditLoggingPermissions.EntityChange.Default, L("Permission:AuditLogging.EntityChanges"));
        // entityChanges.AddChild(AuditLoggingPermissions.EntityChange.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}
