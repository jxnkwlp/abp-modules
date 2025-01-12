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
        builder.Entity<FileContainerAccess>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileContainerAccesses");
        builder.Entity<FileItem>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileItems");
        builder.Entity<FileAccessToken>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileAccessTokens");
        builder.Entity<FileTags>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileTags");
        builder.Entity<FilePath>(options => options.CollectionName = FileManagementDbProperties.DbTablePrefix + "FilePaths");
    }
}
