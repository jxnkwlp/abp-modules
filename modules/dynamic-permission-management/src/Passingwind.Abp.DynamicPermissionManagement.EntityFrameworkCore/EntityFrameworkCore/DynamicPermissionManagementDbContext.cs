using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DynamicPermissionManagement.Permissions;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
public class DynamicPermissionManagementDbContext : AbpDbContext<DynamicPermissionManagementDbContext>, IDynamicPermissionManagementDbContext
{
    public DbSet<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; set; }
    public DbSet<DynamicPermissionDefinition> DynamicPermissions { get; set; }

    public DynamicPermissionManagementDbContext(DbContextOptions<DynamicPermissionManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDynamicPermissionManagement();
    }
}
