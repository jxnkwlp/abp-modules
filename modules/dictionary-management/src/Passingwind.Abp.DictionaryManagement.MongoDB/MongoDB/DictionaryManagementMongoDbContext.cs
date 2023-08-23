using MongoDB.Driver;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DictionaryManagement.MongoDB;

[ConnectionStringName(DictionaryManagementDbProperties.ConnectionStringName)]
public class DictionaryManagementMongoDbContext : AbpMongoDbContext, IDictionaryManagementMongoDbContext
{
    public IMongoCollection<DictionaryGroup> DictionaryGroups => Collection<DictionaryGroup>();
    public IMongoCollection<DictionaryItem> DictionaryItems => Collection<DictionaryItem>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureDictionaryManagement();
    }
}
