using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

[ConnectionStringName(DictionaryManagementDbProperties.ConnectionStringName)]
public class DictionaryManagementDbContext : AbpDbContext<DictionaryManagementDbContext>, IDictionaryManagementDbContext
{
    public DbSet<DictionaryGroup> DictionaryGroups { get; set; }
    public DbSet<DictionaryItem> DictionaryItems { get; set; }

    public DictionaryManagementDbContext(DbContextOptions<DictionaryManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDictionaryManagement();
    }
}
