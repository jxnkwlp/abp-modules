using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using IPermissionDefinitionRecordRepository = Passingwind.Abp.DynamicPermissionManagement.Permissions.IPermissionDefinitionRecordRepository;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore.Repositories;

public class PermissionDefinitionRecordRepository : EfCoreRepository<IPermissionManagementDbContext, PermissionDefinitionRecord, Guid>, IPermissionDefinitionRecordRepository
{
    public PermissionDefinitionRecordRepository(IDbContextProvider<IPermissionManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
