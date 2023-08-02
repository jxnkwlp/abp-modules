using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public interface IFileManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    DbSet<FileContainer> FileContainers { get; }
    DbSet<File> Files { get; }
    DbSet<FileAccessToken> FileAccessTokens { get; }
}
