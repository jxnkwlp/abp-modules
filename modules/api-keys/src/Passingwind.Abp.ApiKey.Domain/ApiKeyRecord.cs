using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyRecord : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Secret { get; set; } = null!;

    public DateTime? ExpirationTime { get; set; }

    public Guid? TenantId { get; protected set; }

    protected ApiKeyRecord()
    {
    }

    public ApiKeyRecord(Guid id, Guid userId, string name, string secret, DateTime? expirationTime = null, Guid? tenantId = null) : base(id)
    {
        UserId = userId;
        Name = name;
        ExpirationTime = expirationTime;
        Secret = secret;
        TenantId = tenantId;
    }
}
