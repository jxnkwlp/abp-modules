using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.Identity;

public class IdentitySecurityLogPagedListRequestDto : PagedAndSortedResultRequestDto
{
    public string? ApplicationName { get; set; }
    public string? Identity { get; set; }
    public string? Action { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? ClientId { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
