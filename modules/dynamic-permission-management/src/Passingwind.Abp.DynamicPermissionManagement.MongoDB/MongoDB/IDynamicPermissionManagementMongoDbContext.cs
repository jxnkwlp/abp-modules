using MongoDB.Driver;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
public interface IDynamicPermissionManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; }
    IMongoCollection<DynamicPermissionDefinition> DynamicPermissions { get; }
}
