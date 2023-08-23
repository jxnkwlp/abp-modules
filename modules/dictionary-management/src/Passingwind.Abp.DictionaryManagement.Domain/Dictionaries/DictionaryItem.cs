using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryItem : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; protected set; } = null!;
    public string DisplayName { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsEnabled { get; set; }
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
    public Guid? TenantId { get; protected set; }

    protected DictionaryItem()
    {
    }

    public DictionaryItem(Guid id, string name, string displayName, string? groupName = null, int displayOrder = 0,
        bool isEnabled = true, string? description = null, Guid? tenantId = null) : base(id)
    {
        GroupName = groupName;
        Name = name;
        DisplayName = displayName;
        DisplayOrder = displayOrder;
        IsEnabled = isEnabled;
        Description = description;
        TenantId = tenantId;
    }
}
