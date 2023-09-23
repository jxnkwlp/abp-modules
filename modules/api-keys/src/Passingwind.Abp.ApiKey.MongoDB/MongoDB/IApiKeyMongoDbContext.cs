using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.ApiKey.MongoDB;

[ConnectionStringName(ApiKeyDbProperties.ConnectionStringName)]
public interface IApiKeyMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<ApiKeyRecord> ApiKeys { get; }
}
