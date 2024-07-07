using System;

namespace Passingwind.Abp.PermissionManagement.Eto;

public class DynamicPermissionGroupDefinitionNameChangedEto
{
    public DynamicPermissionGroupDefinitionNameChangedEto(Guid id, string oldName, string name, string oldTargetName, string targetName, string displayName)
    {
        Id = id;
        OldName = oldName;
        Name = name;
        OldTargetName = oldTargetName;
        TargetName = targetName;
        DisplayName = displayName;
    }

    public Guid Id { get; }
    public string OldName { get; }
    public string Name { get; }
    public string OldTargetName { get; }
    public string TargetName { get; }
    public string DisplayName { get; }
}
