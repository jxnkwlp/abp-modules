using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.PermissionManagement.MongoDB;

public static class PermissionManagementMongoDbContextExtensions
{
    public static void ConfigurePermissionManagementV2(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DynamicPermissionGroupDefinition>(b => b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissionGroups");
        builder.Entity<DynamicPermissionDefinition>(b => b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissions");
    }
}
