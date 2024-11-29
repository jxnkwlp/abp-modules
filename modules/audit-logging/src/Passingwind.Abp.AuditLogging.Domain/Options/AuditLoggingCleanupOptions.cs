namespace Passingwind.Abp.AuditLogging.Options;

public class AuditLoggingCleanupOptions
{
    /// <summary>
    ///  Default: ./audit-logging-backup/"
    /// </summary>
    public string? BackupToDir { get; set; }

    public AuditLoggingCleanupOptions()
    {
        BackupToDir = "./audit-logging-backup/";
    }
}
