using MongoDB.Driver;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public interface IFileManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<FileContainer> FileContainers { get; }
    IMongoCollection<File> Files { get; }
}
