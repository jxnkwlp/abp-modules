using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.Identity.MongoDB;

public static class IdentityMongoDbContextExtensions
{
    public static void ConfigureIdentityV2(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUserTwoFactor>(b => b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "UserTwoFactors");
    }
}
