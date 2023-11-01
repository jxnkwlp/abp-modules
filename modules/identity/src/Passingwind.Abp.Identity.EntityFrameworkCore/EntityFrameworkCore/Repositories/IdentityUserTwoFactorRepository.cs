using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.Identity.EntityFrameworkCore.Repositories;

public class IdentityUserTwoFactorRepository : EfCoreRepository<IdentityDbContextV2, IdentityUserTwoFactor, Guid>, IIdentityUserTwoFactorRepository
{
    public IdentityUserTwoFactorRepository(IDbContextProvider<IdentityDbContextV2> dbContextProvider) : base(dbContextProvider)
    {
    }
}
