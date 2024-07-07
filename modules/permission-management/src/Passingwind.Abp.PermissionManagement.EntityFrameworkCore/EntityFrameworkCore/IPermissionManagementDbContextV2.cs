using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Data;

namespace Passingwind.Abp.PermissionManagement.EntityFrameworkCore;

[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public interface IPermissionManagementDbContextV2 : Volo.Abp.PermissionManagement.EntityFrameworkCore.IPermissionManagementDbContext
{
    DbSet<DynamicPermissionDefinition> DynamicPermissions { get; }
    DbSet<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; }
}
