using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Passingwind.Abp.IdentityClient;

public class IdentityClientClaimMapDto : EntityDto
{
    [Required]
    [MaxLength(64)]
    public string ClaimType { get; set; } = null!;

    public IdentityClientClaimMapAction Action { get; set; }

    public string? ValueFromType { get; set; }

    public string? RawValue { get; set; }
}
