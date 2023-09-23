using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.ApiKey.EntityFrameworkCore;

[ConnectionStringName(ApiKeyDbProperties.ConnectionStringName)]
public interface IApiKeyDbContext : IEfCoreDbContext
{
    DbSet<ApiKeyRecord> ApiKeys { get; }
}
