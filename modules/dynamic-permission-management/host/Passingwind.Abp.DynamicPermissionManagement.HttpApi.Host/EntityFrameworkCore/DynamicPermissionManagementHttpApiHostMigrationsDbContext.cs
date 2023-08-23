using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore;

public class DynamicPermissionManagementHttpApiHostMigrationsDbContext : AbpDbContext<DynamicPermissionManagementHttpApiHostMigrationsDbContext>
{
    public DynamicPermissionManagementHttpApiHostMigrationsDbContext(DbContextOptions<DynamicPermissionManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDynamicPermissionManagement();
    }
}
