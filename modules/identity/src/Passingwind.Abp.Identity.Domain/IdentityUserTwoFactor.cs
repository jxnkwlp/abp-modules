using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.Identity;

public class IdentityUserTwoFactor : AuditedAggregateRoot<Guid>, IMultiTenant
{
    protected IdentityUserTwoFactor()
    {
    }

    public IdentityUserTwoFactor(Guid id, Guid? tenantId = null) : base(id)
    {
        TenantId = tenantId;
    }

    public string? PreferredProvider { get; set; }

    public bool EmailToken { get; set; }
    public bool PhoneNumberToken { get; set; }

    public Guid? TenantId { get; }
}
