using System;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity;

public class IdentityUserV2Dto : IdentityUserDto
{
    public virtual bool ShouldChangePasswordOnNextLogin { get; set; }
    public virtual bool TwoFactorEnabled { get; set; }
    public virtual bool IsExternal { get; set; }
    public virtual bool IsLockout => LockoutEnabled && LockoutEnd > DateTimeOffset.Now;
}
