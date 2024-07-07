using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionLink : AuditedEntity<Guid>
{
    public string SourceName { get; set; } = null!;
    public string TargetName { get; set; } = null!;

    protected DynamicPermissionLink()
    {
    }

    public DynamicPermissionLink(Guid id, string sourceName, string targetName) : base(id)
    {
        SourceName = sourceName;
        TargetName = targetName;
    }
}
