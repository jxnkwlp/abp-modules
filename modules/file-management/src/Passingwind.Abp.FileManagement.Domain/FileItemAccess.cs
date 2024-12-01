using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Passingwind.Abp.FileManagement;

public class FileItemAccess : AuditedEntity<Guid>, IMultiTenant
{
    public Guid FileId { get; set; }
    public Guid? TenantId { get; }
    public string ProviderName { get; set; } = null!;
    public Guid ProviderId { get; set; }
    public bool Read { get; set; }
    public bool Write { get; set; }
    public bool Delete { get; set; }
}
