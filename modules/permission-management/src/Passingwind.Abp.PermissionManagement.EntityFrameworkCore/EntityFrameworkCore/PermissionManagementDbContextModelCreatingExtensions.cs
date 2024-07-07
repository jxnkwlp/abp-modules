using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement.EntityFrameworkCore;

public static class PermissionManagementDbContextModelCreatingExtensions
{
    public static void ConfigurePermissionManagementV2(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder
            .Entity<DynamicPermissionGroupDefinition>(b =>
            {
                b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissionGroups", AbpPermissionManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(q => q.Name).IsRequired().HasMaxLength(64);
                b.Property(q => q.TargetName).IsRequired().HasMaxLength(64);
                b.Property(q => q.DisplayName).IsRequired().HasMaxLength(64);

                b.HasIndex(q => q.Name).IsUnique();
            })
            .Entity<DynamicPermissionDefinition>(b =>
            {
                b.ToTable(AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissions", AbpPermissionManagementDbProperties.DbSchema);
                b.ConfigureByConvention();

                b.Property(q => q.Name).IsRequired().HasMaxLength(64);
                b.Property(q => q.TargetName).IsRequired().HasMaxLength(64);
                b.Property(q => q.DisplayName).IsRequired().HasMaxLength(64);
                b.Property(q => q.Description).HasMaxLength(256);

                b.HasIndex(q => q.GroupId);
                b.HasIndex(q => q.Name).IsUnique();
                b.HasIndex(q => q.ParentId);
            })
            ;
    }
}
