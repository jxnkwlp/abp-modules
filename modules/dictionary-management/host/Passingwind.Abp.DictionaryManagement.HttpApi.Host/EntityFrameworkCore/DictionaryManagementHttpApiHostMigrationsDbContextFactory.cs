using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Passingwind.Abp.DictionaryManagement.EntityFrameworkCore;

public class DictionaryManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<DictionaryManagementHttpApiHostMigrationsDbContext>
{
    public DictionaryManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DictionaryManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DictionaryManagement"));

        return new DictionaryManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
