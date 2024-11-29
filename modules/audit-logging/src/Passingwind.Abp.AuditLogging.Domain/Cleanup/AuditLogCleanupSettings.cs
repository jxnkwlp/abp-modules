namespace Passingwind.Abp.AuditLogging.Cleanup;

public class AuditLogCleanupSettings
{
    /// <summary>
    ///  Cleanup behavior
    /// </summary>
    public AuditLogCleanupBehavior Behavior { get; set; }
    /// <summary>
    ///  Number of days to keep
    /// </summary>
    public int KeepDays { get; set; }
}
