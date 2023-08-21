using System;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

[Serializable]
public class IdentityClientEto : IMultiTenant
{
    public Guid Id { get; set; }
    public string Name { get; protected set; } = null!;
    public string DisplayName { get; set; } = null!;
    public IdentityProviderType ProviderType { get; set; }
    public string ProviderName { get; set; } = null!;
    public bool IsEnabled { get; set; }
    public Guid? TenantId { get; set; }
}
