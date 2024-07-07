using System;
using Volo.Abp.Domain.Repositories;

namespace Passingwind.Abp.PermissionManagement.DynamicPermissions;

public interface IDynamicPermissionLinkRepository : IRepository<DynamicPermissionLink, Guid>
{
}
