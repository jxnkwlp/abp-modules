using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.IdentityClientManagement.IdentityClients;

public class IdentityClientCreateDto : IdentityClientUpdateDto
{
    [Required]
    [MaxLength(64)]
    [RegularExpression(@"^[A-Za-z0-9\\-\\_]*$")]
    public virtual string Name { get; set; } = null!;
}
