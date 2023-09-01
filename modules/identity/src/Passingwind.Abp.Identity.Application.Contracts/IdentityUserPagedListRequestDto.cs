using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class IdentityUserPagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? EmailAddress { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? OrganizationUnitId { get; set; }
    public bool? IsLockedOut { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsExternal { get; set; }
    public DateTime? MinCreationTime { get; set; }
    public DateTime? MaxCreationTime { get; set; }
}
