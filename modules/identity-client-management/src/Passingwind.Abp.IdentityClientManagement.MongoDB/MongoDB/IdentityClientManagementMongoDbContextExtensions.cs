using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClientManagement.MongoDB;

public static class IdentityClientManagementMongoDbContextExtensions
{
    public static void ConfigureIdentityClientManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityClient>(options => options.CollectionName = IdentityClientManagementDbProperties.DbTablePrefix + "IdentityClients");
    }
}
