using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.ApiKey.MongoDB;

[ConnectionStringName(ApiKeyDbProperties.ConnectionStringName)]
public class ApiKeyMongoDbContext : AbpMongoDbContext, IApiKeyMongoDbContext
{
    public IMongoCollection<ApiKeyRecord> ApiKeys => Collection<ApiKeyRecord>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureApiKey();
    }
}
