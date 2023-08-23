using System;

namespace Passingwind.Abp.DynamicPermissionManagement.Permissions;

public class DynamicPermissionDefinitionListRequestDto
{
    public Guid? GroupId { get; set; }
    public Guid? ParentId { get; set; }
}
