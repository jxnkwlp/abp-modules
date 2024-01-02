using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

public static class FileManagementMongoDbContextExtensions
{
    public static void ConfigureFileManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<FileContainer>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileContainers");
        builder.Entity<FileItem>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "Files");
        builder.Entity<FileAccessToken>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileAccessTokens");
    }
}
