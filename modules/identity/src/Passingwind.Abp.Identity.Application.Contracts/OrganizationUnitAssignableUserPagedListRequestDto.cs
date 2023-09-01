using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitAssignableUserPagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
