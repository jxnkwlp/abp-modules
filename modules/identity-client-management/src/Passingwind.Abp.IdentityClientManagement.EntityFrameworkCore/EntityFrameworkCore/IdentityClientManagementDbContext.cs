using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;

[ConnectionStringName(IdentityClientManagementDbProperties.ConnectionStringName)]
public class IdentityClientManagementDbContext : AbpDbContext<IdentityClientManagementDbContext>, IIdentityClientManagementDbContext
{
    public DbSet<IdentityClient> IdentityClients { get; set; }

    public IdentityClientManagementDbContext(DbContextOptions<IdentityClientManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentityClientManagement();
    }
}
