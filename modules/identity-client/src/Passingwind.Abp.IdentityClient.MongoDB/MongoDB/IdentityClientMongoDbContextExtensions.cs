using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClient.MongoDB;

public static class IdentityClientMongoDbContextExtensions
{
    public static void ConfigureIdentityClient(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityClient>(options => options.CollectionName = IdentityClientDbProperties.DbTablePrefix + "IdentityClients");
    }
}
