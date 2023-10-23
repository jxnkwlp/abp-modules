using System;
using Volo.Abp.Domain.Entities;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientClaimMap : Entity
{
    public Guid IdentityClientId { get; set; }

    public string ClaimType { get; set; } = null!;

    public IdentityClientClaimMapAction Action { get; set; }

    public string? ValueFromType { get; set; }

    public string? RawValue { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { IdentityClientId, ClaimType };
    }
}
