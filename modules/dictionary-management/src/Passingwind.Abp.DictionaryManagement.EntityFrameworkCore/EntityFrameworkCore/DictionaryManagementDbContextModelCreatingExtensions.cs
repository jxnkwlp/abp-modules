using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

public static class DictionaryManagementDbContextModelCreatingExtensions
{
    public static void ConfigureDictionaryManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder
            .Entity<DictionaryGroup>(b =>
            {
                b.ToTable(DictionaryManagementDbProperties.DbTablePrefix + "DictionaryGroups", DictionaryManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(DictionaryManagementConsts.MaxNameLength);
                b.Property(x => x.DisplayName).IsRequired().HasMaxLength(DictionaryManagementConsts.MaxDisplayNameLength);
                b.Property(x => x.ParentName).HasMaxLength(DictionaryManagementConsts.MaxNameLength);
                b.Property(x => x.Description).HasMaxLength(DictionaryManagementConsts.MaxDescriptionLength);

                b.HasIndex(x => x.Name).IsUnique();
                b.HasIndex(x => x.ParentName);
                b.HasIndex(x => x.CreationTime).IsDescending();
            })
            .Entity<DictionaryItem>(b =>
            {
                b.ToTable(DictionaryManagementDbProperties.DbTablePrefix + "DictionaryItems", DictionaryManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(DictionaryManagementConsts.MaxNameLength);
                b.Property(x => x.DisplayName).IsRequired().HasMaxLength(DictionaryManagementConsts.MaxDisplayNameLength);
                b.Property(x => x.GroupName).HasMaxLength(DictionaryManagementConsts.MaxNameLength);
                b.Property(x => x.Description).HasMaxLength(DictionaryManagementConsts.MaxDescriptionLength);

                b.HasIndex(x => x.Name).IsUnique();
                b.HasIndex(x => x.GroupName);
                b.HasIndex(x => x.CreationTime).IsDescending();
            });
    }
}
