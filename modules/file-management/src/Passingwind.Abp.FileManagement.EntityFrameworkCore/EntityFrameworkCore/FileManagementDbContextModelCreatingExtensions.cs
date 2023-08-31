using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.FileManagement.Files;
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

                b.Property(q => q.Name).IsRequired().HasMaxLength(FileManagementConsts.MaxFileContainerNameLength);
                b.Property(q => q.Description).HasMaxLength(FileManagementConsts.MaxFileContainerDescriptionLength);

                b.HasIndex(q => q.Name);
            })
            .Entity<File>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "Files", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(q => q.FileName).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemFileNameLength);
                b.Property(q => q.MimeType).HasMaxLength(FileManagementConsts.MaxFileItemMimeTypeLength);
                b.Property(q => q.BlobName).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemBlobNameLength);
                b.Property(q => q.Hash).HasMaxLength(FileManagementConsts.MaxFileItemHashLength);
                b.Property(q => q.UniqueId).IsRequired().HasMaxLength(FileManagementConsts.MaxFileItemUniqueIdLength);

                b.HasIndex(q => q.ContainerId);
                b.HasIndex(q => q.FileName);
                b.HasIndex(q => q.Hash);
                b.HasIndex(q => q.UniqueId);
                b.HasIndex(q => q.CreationTime).IsDescending();
            })
            .Entity<FileAccessToken>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileAccessTokens", FileManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(q => q.Token).IsRequired().HasMaxLength(FileManagementConsts.MaxFileAccessTokenTokenLength);

                b.HasIndex(q => q.Token);
                b.HasIndex(q => q.FileId);
            });
    }
}
