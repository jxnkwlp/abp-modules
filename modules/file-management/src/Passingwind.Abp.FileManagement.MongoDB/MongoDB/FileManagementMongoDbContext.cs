using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB;

[ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
public class FileManagementMongoDbContext : AbpMongoDbContext, IJadeFileManagementMongoDbContext
{
    public IMongoCollection<FileContainer> FileContainers => Collection<FileContainer>();
    public IMongoCollection<FileItem> Files => Collection<FileItem>();
    public IMongoCollection<FileAccessToken> FileAccessTokens => Collection<FileAccessToken>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureFileManagement();
    }
}
