using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class IdentityClaimTypePagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
