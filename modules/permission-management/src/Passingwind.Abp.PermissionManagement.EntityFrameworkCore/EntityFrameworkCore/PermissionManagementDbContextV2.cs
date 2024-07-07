using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.PermissionManagement.DynamicPermissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Passingwind.Abp.PermissionManagement.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
[ReplaceDbContext(typeof(Volo.Abp.PermissionManagement.EntityFrameworkCore.IPermissionManagementDbContext))]
public class PermissionManagementDbContextV2 : AbpDbContext<PermissionManagementDbContextV2>, IPermissionManagementDbContextV2
{
    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    public DbSet<DynamicPermissionDefinition> DynamicPermissions { get; set; }
    public DbSet<DynamicPermissionGroupDefinition> DynamicPermissionGroups { get; set; }

    public PermissionManagementDbContextV2(DbContextOptions<PermissionManagementDbContextV2> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigurePermissionManagementV2();
    }
}
