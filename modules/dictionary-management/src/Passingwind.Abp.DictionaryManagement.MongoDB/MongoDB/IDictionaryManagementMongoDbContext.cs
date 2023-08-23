using MongoDB.Driver;
using Passingwind.Abp.DictionaryManagement.Dictionaries;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.DictionaryManagement.MongoDB;

[ConnectionStringName(DictionaryManagementDbProperties.ConnectionStringName)]
public interface IDictionaryManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<DictionaryGroup> DictionaryGroups { get; }
    IMongoCollection<DictionaryItem> DictionaryItems { get; }
}
