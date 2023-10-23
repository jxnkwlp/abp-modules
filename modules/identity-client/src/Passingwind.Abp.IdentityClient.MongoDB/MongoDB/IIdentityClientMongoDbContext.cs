using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClient.MongoDB;

[ConnectionStringName(IdentityClientDbProperties.ConnectionStringName)]
public interface IIdentityClientMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<IdentityClient> IdentityClients { get; }
}
