using MongoDB.Driver;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.IdentityClientManagement.MongoDB;

[ConnectionStringName(IdentityClientManagementDbProperties.ConnectionStringName)]
public interface IIdentityClientManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<IdentityClient> IdentityClients { get; }
}
