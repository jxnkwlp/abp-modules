using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClient.MongoDB;

[ConnectionStringName(IdentityClientDbProperties.ConnectionStringName)]
public class IdentityClientMongoDbContext : AbpMongoDbContext, IIdentityClientMongoDbContext
{
    public IMongoCollection<IdentityClient> IdentityClients => Collection<IdentityClient>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureIdentityClient();
    }
}
