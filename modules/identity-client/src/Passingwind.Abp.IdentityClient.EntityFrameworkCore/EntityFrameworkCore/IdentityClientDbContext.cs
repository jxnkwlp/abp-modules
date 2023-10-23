using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClient.EntityFrameworkCore;

[ConnectionStringName(IdentityClientDbProperties.ConnectionStringName)]
public class IdentityClientDbContext : AbpDbContext<IdentityClientDbContext>, IIdentityClientDbContext
{
    public DbSet<IdentityClient> IdentityClients { get; set; }

    public IdentityClientDbContext(DbContextOptions<IdentityClientDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentityClient();
    }
}
