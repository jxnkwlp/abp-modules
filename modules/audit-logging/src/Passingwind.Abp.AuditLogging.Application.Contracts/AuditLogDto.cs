using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.AuditLogging;

public class AuditLogDto : EntityDto<Guid>, IMultiTenant
{
    public virtual string? ApplicationName { get; set; }

    public virtual Guid? UserId { get; set; }

    public virtual string? UserName { get; set; }

    public virtual Guid? TenantId { get; set; }

    public virtual string? TenantName { get; set; }

    public virtual Guid? ImpersonatorUserId { get; set; }

    public virtual string? ImpersonatorUserName { get; set; }

    public virtual Guid? ImpersonatorTenantId { get; set; }

    public virtual string? ImpersonatorTenantName { get; set; }

    public virtual DateTime ExecutionTime { get; set; }

    public virtual int ExecutionDuration { get; set; }

    public virtual string? ClientIpAddress { get; set; }

    public virtual string? ClientName { get; set; }

    public virtual string? ClientId { get; set; }

    public virtual string? CorrelationId { get; set; }

    public virtual string? BrowserInfo { get; set; }

    public virtual string? HttpMethod { get; set; }

    public virtual string? Url { get; set; }

    public virtual string? Exceptions { get; set; }

    public virtual string? Comments { get; set; }

    public virtual int? HttpStatusCode { get; set; }

    public virtual List<AuditLogActionDto>? Actions { get; set; }

    public virtual List<EntityChangeDto>? EntityChanges { get; set; }
}
