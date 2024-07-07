using System;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinitionListRequestDto
{
    public Guid? GroupId { get; set; }
    public Guid? ParentId { get; set; }
}
