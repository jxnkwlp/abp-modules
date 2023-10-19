using System;
using System.ComponentModel.DataAnnotations;

namespace Passingwind.Abp.Identity;

public class IdentityUserBatchLockDto : IdentityUserBatchInputDto
{
    [Required]
    public DateTime EndTime { get; set; }
}
