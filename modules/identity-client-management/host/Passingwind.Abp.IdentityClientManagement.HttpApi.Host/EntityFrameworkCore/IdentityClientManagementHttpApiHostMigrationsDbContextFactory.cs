using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;

public class IdentityClientManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<IdentityClientManagementHttpApiHostMigrationsDbContext>
{
    public IdentityClientManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<IdentityClientManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("IdentityClientManagement"));

        return new IdentityClientManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
