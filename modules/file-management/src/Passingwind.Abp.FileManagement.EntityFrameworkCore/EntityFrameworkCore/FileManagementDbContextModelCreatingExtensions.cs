using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

public static class FileManagementDbContextModelCreatingExtensions
{
    public static void ConfigureFileManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder
            .Entity<FileContainer>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileContainers", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(FileManagementConsts.MaxFileContainerNameLength);
                b.Property(x => x.Description).HasMaxLength(FileManagementConsts.MaxFileContainerDescriptionLength);

                b.HasMany(x => x.Accesses).WithOne().HasForeignKey(x => x.FileContainerId);

                b.HasIndex(x => x.Name);
            })
            .Entity<FileContainerAccess>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileContainerAccesses", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.ProviderName).IsRequired().HasMaxLength(64);
                b.Property(x => x.ProviderKey).IsRequired().HasMaxLength(64);

                b.HasKey(x => new { x.FileContainerId, x.ProviderName, x.ProviderKey });
            })
            .Entity<FileItem>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileItems", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.FileName).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemFileNameLength);
                b.Property(x => x.MimeType).HasMaxLength(FileManagementConsts.MaxFileItemMimeTypeLength);
                b.Property(x => x.BlobName).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemBlobNameLength);
                b.Property(x => x.Hash).HasMaxLength(FileManagementConsts.MaxFileItemHashLength);
                b.Property(x => x.UniqueId).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemUniqueIdLength);

                b.HasMany(x => x.Tags).WithOne().HasForeignKey(x => x.FileId);
                b.HasOne(x => x.Path).WithOne().HasForeignKey<FilePath>(x => x.FileId);

                b.HasIndex(x => x.ContainerId);
                b.HasIndex(x => x.FileName);
                b.HasIndex(x => x.Hash);
                b.HasIndex(x => x.UniqueId);
                b.HasIndex(x => x.CreationTime).IsDescending();
            })
            .Entity<FileTags>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileTags", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(FileManagementConsts.MaxFileTagValueLength);

                b.HasKey(x => new { x.FileId, x.Name });
            })
            .Entity<FilePath>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FilePaths", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.FullPath).IsRequired();

                b.HasKey(x => new { x.FileId });
            })
            .Entity<FileAccessToken>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileAccessTokens", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Token).IsRequired().HasMaxLength(FileManagementConsts.MaxFileAccessTokenTokenLength);

                b.Property(x => x.FileName).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemFileNameLength);
                b.Property(x => x.MimeType).HasMaxLength(FileManagementConsts.MaxFileItemMimeTypeLength);

                b.HasIndex(x => x.Token);
                b.HasIndex(x => x.FileId);
            });
    }
}
