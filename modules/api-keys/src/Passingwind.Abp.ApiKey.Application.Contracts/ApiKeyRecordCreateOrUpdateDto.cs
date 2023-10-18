using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace Passingwind.Abp.ApiKey;

public class ApiKeyRecordCreateOrUpdateDto : ExtensibleEntityDto
{
    [Required]
    [DynamicMaxLength(typeof(ApiKeyConsts), nameof(ApiKeyConsts.MaxApiKeyNameLength))]
    public virtual string Name { get; set; } = null!;

    public virtual DateTime? ExpirationTime { get; set; }
}
