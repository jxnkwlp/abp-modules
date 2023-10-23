using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientConfigurationDto : EntityDto
{
    public virtual string Name { get; set; } = null!;
    public virtual string? Value { get; set; }
}
