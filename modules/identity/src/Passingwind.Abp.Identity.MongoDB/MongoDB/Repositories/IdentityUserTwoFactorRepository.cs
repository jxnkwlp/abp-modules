using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.Identity.MongoDB.Repositories;

public class IdentityUserTwoFactorRepository : MongoDbRepository<IdentityMongoDbContextV2, IdentityUserTwoFactor, Guid>, IIdentityUserTwoFactorRepository
{
    public IdentityUserTwoFactorRepository(IMongoDbContextProvider<IdentityMongoDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }
}
