using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DictionaryManagement.MongoDB;

public static class DictionaryManagementMongoDbContextExtensions
{
    public static void ConfigureDictionaryManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DictionaryGroup>(options => options.CollectionName = DictionaryManagementDbProperties.DbTablePrefix + "DictionaryGroups");
        builder.Entity<DictionaryItem>(options => options.CollectionName = DictionaryManagementDbProperties.DbTablePrefix + "DictionaryItems");
    }
}
