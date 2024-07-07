using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinition : AuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; protected set; } = null!;
    [NotNull]
    public virtual string TargetName { get; protected set; } = null!;
    [NotNull]
    public virtual string DisplayName { get; set; } = null!;

    public virtual Guid GroupId { get; protected set; }
    public virtual bool IsEnabled { get; set; }
    public virtual Guid? ParentId { get; protected set; }
    public virtual string? Description { get; set; }

    protected DynamicPermissionDefinition()
    {
    }

    public DynamicPermissionDefinition(Guid id, string name, string targetName, string displayName, bool isEnabled = true, Guid groupId = default, Guid? parentId = null) : base(id)
    {
        Name = name;
        TargetName = targetName;
        DisplayName = displayName;
        IsEnabled = isEnabled;
        GroupId = groupId;
        ParentId = parentId;
    }
}
