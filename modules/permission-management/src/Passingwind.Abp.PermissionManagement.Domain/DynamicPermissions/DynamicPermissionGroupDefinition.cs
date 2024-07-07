using System;
using JetBrains.Annotations;
using Passingwind.Abp.PermissionManagement.Eto;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionGroupDefinition : AuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; protected set; } = null!;
    [NotNull]
    public virtual string TargetName { get; protected set; } = null!;
    [NotNull]
    public virtual string DisplayName { get; set; } = null!;

    protected DynamicPermissionGroupDefinition()
    {
    }

    public DynamicPermissionGroupDefinition(Guid id, string name, string targetName, string displayName) : base(id)
    {
        Name = name;
        TargetName = targetName;
        DisplayName = displayName;
    }

    public void SetName(string name, string targetName)
    {
        if (name == Name)
            return;

        string oldName = Name;
        Name = name;

        var oldTargetName = TargetName;
        TargetName = targetName;

        AddDistributedEvent(new DynamicPermissionGroupDefinitionNameChangedEto(
            id: Id,
            oldName: oldName,
            name: Name,
            oldTargetName: oldTargetName,
            targetName: targetName,
            displayName: DisplayName));
    }
}
