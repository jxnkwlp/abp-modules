using MongoDB.Driver;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Passingwind.Abp.PermissionManagement.MongoDB;

[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
[ReplaceDbContext(typeof(IPermissionManagementMongoDbContext))]
public class PermissionManagementMongoDbContextV2 : AbpMongoDbContext, IPermissionManagementMongoDbContextV2
{
    public IMongoCollection<PermissionGroupDefinitionRecord> PermissionGroups => Collection<PermissionGroupDefinitionRecord>();
    public IMongoCollection<PermissionDefinitionRecord> Permissions => Collection<PermissionDefinitionRecord>();
    public IMongoCollection<PermissionGrant> PermissionGrants => Collection<PermissionGrant>();

    public IMongoCollection<DynamicPermissionDefinition> DynamicPermissions => Collection<DynamicPermissionDefinition>();
    public IMongoCollection<DynamicPermissionGroupDefinition> DynamicPermissionGroups => Collection<DynamicPermissionGroupDefinition>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigurePermissionManagementV2();
    }
}
