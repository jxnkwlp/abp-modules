using MongoDB.Driver;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
public class DynamicPermissionManagementMongoDbContext : AbpMongoDbContext, IDynamicPermissionManagementMongoDbContext
{
    public IMongoCollection<DynamicPermissionGroupDefinition> DynamicPermissionGroups => Collection<DynamicPermissionGroupDefinition>();
    public IMongoCollection<DynamicPermissionDefinition> DynamicPermissions => Collection<DynamicPermissionDefinition>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureDynamicPermissionManagement();
    }
}
