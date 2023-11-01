using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.Identity.MongoDB;

[ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
public interface IIdentityMongoDbContextV2 : IAbpMongoDbContext
{
    IMongoCollection<IdentityUserTwoFactor> UserTwoFactors { get; }
}
