using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientConfigurationDto : EntityDto
{
    public virtual string Name { get; set; } = null!;
    public virtual string? Value { get; set; }
}
