using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB;

public static class DynamicPermissionManagementMongoDbContextExtensions
{
    public static void ConfigureDynamicPermissionManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DynamicPermissionGroupDefinition>(b => b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissionGroups");
        builder.Entity<DynamicPermissionDefinition>(b => b.CollectionName = AbpPermissionManagementDbProperties.DbTablePrefix + "DynamicPermissions");
    }
}
