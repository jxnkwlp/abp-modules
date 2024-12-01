using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public class FileManagementDbContext : AbpDbContext<FileManagementDbContext>, IFileManagementDbContext
{
    public DbSet<FileContainer> FileContainers { get; set; }
    public DbSet<FileItem> Files { get; set; }
    public DbSet<FileAccessToken> FileAccessTokens { get; set; }

    public FileManagementDbContext(DbContextOptions<FileManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureFileManagement();
    }
}
