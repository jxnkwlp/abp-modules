using MongoDB.Driver;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public class FileManagementMongoDbContext : AbpMongoDbContext, IFileManagementMongoDbContext
{
    public IMongoCollection<FileContainer> FileContainers => Collection<FileContainer>();
    public IMongoCollection<File> Files => Collection<File>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureFileManagement();
    }
}
