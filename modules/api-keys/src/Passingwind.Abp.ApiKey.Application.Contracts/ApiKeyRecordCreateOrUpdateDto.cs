using System;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyRecordCreateOrUpdateDto : ExtensibleEntityDto
{
    public virtual string Name { get; set; } = null!;
    public virtual DateTime? ExpirationTime { get; set; }
}
