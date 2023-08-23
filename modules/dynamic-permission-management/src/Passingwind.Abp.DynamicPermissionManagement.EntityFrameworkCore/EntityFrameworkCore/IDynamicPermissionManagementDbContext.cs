using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
public interface IDynamicPermissionManagementDbContext : IEfCoreDbContext
{
    DbSet<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; }
    DbSet<DynamicPermissionDefinition> DynamicPermissions { get; }
}
