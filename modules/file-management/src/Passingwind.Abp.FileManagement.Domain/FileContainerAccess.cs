using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Passingwind.Abp.FileManagement;

public class FileContainerAccess : AuditedEntity
{
    public Guid FileContainerId { get; set; }
    public string ProviderName { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;

    public bool Read { get; set; }
    public bool Write { get; set; }
    public bool Delete { get; set; }

    public override object[] GetKeys()
    {
        return [FileContainerId, ProviderName, ProviderKey];
    }
}
