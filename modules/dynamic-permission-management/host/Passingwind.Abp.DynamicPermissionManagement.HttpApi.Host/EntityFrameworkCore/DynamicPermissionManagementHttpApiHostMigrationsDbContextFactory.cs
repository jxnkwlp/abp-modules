using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore;

public class DynamicPermissionManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<DynamicPermissionManagementHttpApiHostMigrationsDbContext>
{
    public DynamicPermissionManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DynamicPermissionManagementHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("DynamicPermissionManagement"));

        return new DynamicPermissionManagementHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
