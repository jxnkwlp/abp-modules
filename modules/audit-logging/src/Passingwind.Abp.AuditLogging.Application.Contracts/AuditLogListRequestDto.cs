using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.AuditLogging;

public class AuditLogListRequestDto : PagedResultRequestDto
{
    public virtual string? Sorting { get; set; }
    public virtual string? HttpMethod { get; set; }
    public virtual int? HttpStatusCode { get; set; }
    public virtual string? Url { get; set; }
    public virtual DateTime? StartTime { get; set; }
    public virtual DateTime? EndTime { get; set; }
    public virtual string? ClientId { get; set; }
    public virtual Guid? UserId { get; set; }
    public virtual string? UserName { get; set; }
    public virtual string? ApplicationName { get; set; }
    public virtual string? ClientIpAddress { get; set; }
    public virtual string? CorrelationId { get; set; }
    public virtual int? MaxExecutionDuration { get; set; }
    public virtual int? MinExecutionDuration { get; set; }
    public virtual bool? HasException { get; set; }
}
