using System;

namespace Passingwind.Abp.PermissionManagement.Eto;

public class DynamicPermissionDefinitionNameChangedEto
{
    public DynamicPermissionDefinitionNameChangedEto(Guid id, string oldName, string name, string oldTagetName, string tagetName)
    {
        Id = id;
        OldName = oldName;
        Name = name;
        OldTagetName = oldTagetName;
        TagetName = tagetName;
    }

    public Guid Id { get; }
    public string OldName { get; }
    public string Name { get; }
    public string OldTagetName { get; }
    public string TagetName { get; }
}
