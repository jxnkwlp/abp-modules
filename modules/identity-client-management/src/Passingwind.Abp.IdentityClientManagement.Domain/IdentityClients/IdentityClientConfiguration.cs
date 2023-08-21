using System;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientConfiguration : Entity
{
    public Guid IdentityClientId { get; set; }

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { IdentityClientId, Name };
    }
}
