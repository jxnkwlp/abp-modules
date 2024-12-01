using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public interface IFileManagementDbContext : IEfCoreDbContext
{
    DbSet<FileContainer> FileContainers { get; }
    DbSet<FileItem> Files { get; }
    DbSet<FileAccessToken> FileAccessTokens { get; }
}
