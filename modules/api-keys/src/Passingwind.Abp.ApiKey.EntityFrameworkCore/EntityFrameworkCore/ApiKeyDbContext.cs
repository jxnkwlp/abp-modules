using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.ApiKey.EntityFrameworkCore;

[ConnectionStringName(ApiKeyDbProperties.ConnectionStringName)]
public class ApiKeyDbContext : AbpDbContext<ApiKeyDbContext>, IApiKeyDbContext
{
    public DbSet<ApiKeyRecord> ApiKeys { get; set; }

    public ApiKeyDbContext(DbContextOptions<ApiKeyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureApiKey();
    }
}
