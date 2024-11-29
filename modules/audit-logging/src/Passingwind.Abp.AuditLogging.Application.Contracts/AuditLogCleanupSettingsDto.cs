namespace Passingwind.Abp.AuditLogging;

public class AuditLogCleanupSettingsDto
{
    public AuditLogCleanupBehavior Behavior { get; set; }
    public string BehaviorDescription => Behavior.ToString();
    public int KeepDays { get; set; }
}
