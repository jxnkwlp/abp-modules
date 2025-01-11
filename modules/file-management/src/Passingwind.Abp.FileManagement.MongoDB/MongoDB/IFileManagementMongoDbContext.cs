using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public interface IFileManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<FileContainer> FileContainers { get; }
    IMongoCollection<FileItem> Files { get; }
    IMongoCollection<FileAccessToken> FileAccessTokens { get; }
}
