using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.DictionaryManagement.Dictionaries;

public class DictionaryGroup : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; protected set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? ParentName { get; protected set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public Guid? TenantId { get; protected set; }

    protected DictionaryGroup()
    {
    }

    public DictionaryGroup(Guid id, string name, string displayName, string? parentName = null, string? description = null,
        bool isPublic = false, Guid? tenantId = null) : base(id)
    {
        ParentName = parentName;
        Name = name;
        DisplayName = displayName;
        Description = description;
        IsPublic = isPublic;
        TenantId = tenantId;
    }
}
