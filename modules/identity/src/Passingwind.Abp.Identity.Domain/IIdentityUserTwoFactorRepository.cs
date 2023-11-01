using System;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.Identity;

public interface IIdentityUserTwoFactorRepository : IBasicRepository<IdentityUserTwoFactor, Guid>
{
}
