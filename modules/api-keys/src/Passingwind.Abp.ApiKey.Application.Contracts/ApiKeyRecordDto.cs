using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyRecordDto : ExtensibleAuditedEntityDto<Guid>
{
    public virtual string Name { get; set; } = null!;
    public virtual DateTime? ExpirationTime { get; set; }
    public virtual string Secret { get; set; } = null!;
}
