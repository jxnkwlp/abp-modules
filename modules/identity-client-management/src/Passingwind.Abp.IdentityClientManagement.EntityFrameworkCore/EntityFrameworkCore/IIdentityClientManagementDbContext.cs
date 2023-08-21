using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.IdentityClientManagement.IdentityClients;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.IdentityClientManagement.EntityFrameworkCore;

[ConnectionStringName(IdentityClientManagementDbProperties.ConnectionStringName)]
public interface IIdentityClientManagementDbContext : IEfCoreDbContext
{
    DbSet<IdentityClient> IdentityClients { get; }
}
