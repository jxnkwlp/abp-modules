using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

public class DictionaryManagementHttpApiHostMigrationsDbContext : AbpDbContext<DictionaryManagementHttpApiHostMigrationsDbContext>
{
    public DictionaryManagementHttpApiHostMigrationsDbContext(DbContextOptions<DictionaryManagementHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDictionaryManagement();
    }
}
