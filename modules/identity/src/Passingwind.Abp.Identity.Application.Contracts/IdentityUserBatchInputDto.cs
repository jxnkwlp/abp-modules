using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class IdentityUserBatchInputDto
{
    [Required]
    public IEnumerable<Guid> UserIds { get; set; } = null!;
}
