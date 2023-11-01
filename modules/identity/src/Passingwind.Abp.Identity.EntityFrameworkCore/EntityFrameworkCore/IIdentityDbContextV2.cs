using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Passingwind.Abp.Identity.EntityFrameworkCore;

[ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
public interface IIdentityDbContextV2 : IIdentityDbContext
{
    DbSet<IdentityUserTwoFactor> UserTwoFactors { get; }
}
