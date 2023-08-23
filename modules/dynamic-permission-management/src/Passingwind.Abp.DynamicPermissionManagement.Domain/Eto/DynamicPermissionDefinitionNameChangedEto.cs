using System;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;

namespace Passingwind.Abp.DynamicPermissionManagement.Eto;

public class DynamicPermissionDefinitionNameChangedEto
{
    public DynamicPermissionDefinitionNameChangedEto(Guid id, string oldName, string name, DynamicPermissionDefinition entity)
    {
        Id = id;
        OldName = oldName;
        Name = name;
        Entity = entity;
    }

    public Guid Id { get; }
    public string OldName { get; }
    public string Name { get; }
    public DynamicPermissionDefinition Entity { get; }
}
