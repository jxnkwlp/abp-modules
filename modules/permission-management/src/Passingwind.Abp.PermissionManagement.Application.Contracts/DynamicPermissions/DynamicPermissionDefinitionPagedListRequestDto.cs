using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public class DynamicPermissionDefinitionPagedListRequestDto : PagedResultRequestDto
{
    public Guid? GroupId { get; set; }
    public Guid? ParentId { get; set; }
}
