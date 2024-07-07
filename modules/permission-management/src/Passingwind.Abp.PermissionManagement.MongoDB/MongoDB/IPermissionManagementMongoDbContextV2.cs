using MongoDB.Driver;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Data;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Passingwind.Abp.PermissionManagement.MongoDB;

[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public interface IPermissionManagementMongoDbContextV2 : IPermissionManagementMongoDbContext
{
    IMongoCollection<DynamicPermissionDefinition> DynamicPermissions { get; }
    IMongoCollection<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; }
}
