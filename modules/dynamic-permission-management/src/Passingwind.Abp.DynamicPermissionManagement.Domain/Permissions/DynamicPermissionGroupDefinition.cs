using System;
using JetBrains.Annotations;
using Passingwind.Abp.DynamicPermissionManagement.Eto;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionGroupDefinition : AuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; protected set; } = null!;
    [NotNull]
    public virtual string DisplayName { get; set; } = null!;

    protected DynamicPermissionGroupDefinition()
    {
    }

    public DynamicPermissionGroupDefinition(Guid id, string name, string displayName) : base(id)
    {
        Name = name;
        DisplayName = displayName;
    }

    public void SetName(string value)
    {
        if (value == Name)
            return;

        string oldName = Name;
        Name = value;

        AddDistributedEvent(new DynamicPermissionGroupDefinitionNameChangedEto(Id, oldName, Name, this));
    }
}
