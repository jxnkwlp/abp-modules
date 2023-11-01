using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace Passingwind.Abp.Identity.EntityFrameworkCore;

public static class IdentityDbContextModelCreatingExtensions
{
    public static void ConfigureIdentityV2(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUserTwoFactor>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UserTwoFactors", AbpIdentityDbProperties.DbSchema);
            b.ConfigureByConvention();

            b.Property(q => q.PreferredProvider).HasMaxLength(64);
        });
    }
}
