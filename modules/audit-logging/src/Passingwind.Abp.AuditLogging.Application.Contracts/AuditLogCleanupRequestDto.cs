using System;

namespace Passingwind.Abp.AuditLogging;

public class AuditLogCleanupRequestDto
{
    public AuditLogCleanupBehavior Behavior { get; set; }
    public DateTime EndTime { get; set; }
}
