using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;

public class IdentityClientManagementHttpApiHostMigrationsDbContext : AbpDbContext<IdentityClientManagementHttpApiHostMigrationsDbContext>
{
    public IdentityClientManagementHttpApiHostMigrationsDbContext(DbContextOptions<IdentityClientManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentityClientManagement();
    }
}
