using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionDefinition : AuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Name { get; protected set; } = null!;
    [NotNull]
    public virtual string DisplayName { get; set; } = null!;
    public virtual Guid GroupId { get; protected set; }
    public virtual bool IsEnabled { get; set; }
    public virtual Guid? ParentId { get; protected set; }
    public virtual string? Description { get; set; }

    protected DynamicPermissionDefinition()
    {
    }

    public DynamicPermissionDefinition(Guid id, string name, string displayName, Guid groupId, Guid? parentId = null) : base(id)
    {
        Name = name;
        DisplayName = displayName;
        GroupId = groupId;
        ParentId = parentId;
    }

    //public virtual void SetName(string value)
    //{
    //    if (value == Name)
    //        return;

    //    string oldName = Name;
    //    Name = value;

    //    AddDistributedEvent(new DynamicPermissionDefinitionNameChangedEto(Id, oldName, Name, this));
    //}

    public virtual void SetParentId(Guid? value)
    {
        ParentId = value;
    }
}
