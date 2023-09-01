using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class OrganizationUnitUserPagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
