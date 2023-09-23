using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.ApiKey.MongoDB;

public static class ApiKeyMongoDbContextExtensions
{
    public static void ConfigureApiKey(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<ApiKeyRecord>(b => b.CollectionName = ApiKeyDbProperties.DbTablePrefix + "ApiKeys");
    }
}
