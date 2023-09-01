using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitAssignableRolePagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
