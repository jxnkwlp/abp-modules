using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClient.EntityFrameworkCore;

[ConnectionStringName(IdentityClientDbProperties.ConnectionStringName)]
public interface IIdentityClientDbContext : IEfCoreDbContext
{
    DbSet<IdentityClient> IdentityClients { get; }
}
