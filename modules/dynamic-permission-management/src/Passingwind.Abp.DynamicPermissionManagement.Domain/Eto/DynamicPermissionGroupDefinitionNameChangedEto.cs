using System;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;

namespace Passingwind.Abp.DynamicPermissionManagement.Eto;

public class DynamicPermissionGroupDefinitionNameChangedEto
{
    public DynamicPermissionGroupDefinitionNameChangedEto(Guid id, string oldName, string name, DynamicPermissionGroupDefinition entity)
    {
        Id = id;
        OldName = oldName;
        Name = name;
        Entity = entity;
    }

    public Guid Id { get; }
    public string OldName { get; }
    public string Name { get; }
    public DynamicPermissionGroupDefinition Entity { get; }
}
