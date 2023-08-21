using MongoDB.Driver;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClientManagement.MongoDB;

[ConnectionStringName(IdentityClientManagementDbProperties.ConnectionStringName)]
public class IdentityClientManagementMongoDbContext : AbpMongoDbContext, IIdentityClientManagementMongoDbContext
{
    public IMongoCollection<IdentityClient> IdentityClients => Collection<IdentityClient>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureIdentityClientManagement();
    }
}
