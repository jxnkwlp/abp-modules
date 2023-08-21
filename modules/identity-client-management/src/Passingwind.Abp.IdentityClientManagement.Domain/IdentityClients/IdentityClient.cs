using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClient : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; protected set; } = null!;
    public string DisplayName { get; set; } = null!;
    public IdentityProviderType ProviderType { get; set; }
    public string ProviderName { get; set; } = null!;
    public bool IsEnabled { get; set; } = true;
    public int DisplayOrder { get; set; }
    public bool IsDebugMode { get; set; }
    public Guid? TenantId { get; protected set; }

    public List<IdentityClientConfiguration> Configurations { get; set; } = new List<IdentityClientConfiguration>();

    public List<IdentityClientClaimMap> ClaimMaps { get; set; } = new List<IdentityClientClaimMap>();

    /// <summary>
    ///  (JSON)
    /// </summary>
    public List<string> RequiredClaimTypes { get; set; } = new List<string>();

    protected IdentityClient()
    {
    }

    public IdentityClient(Guid id, string name, Guid? tenantId = null) : base(id)
    {
        Name = name;
        TenantId = tenantId;
    }
}
