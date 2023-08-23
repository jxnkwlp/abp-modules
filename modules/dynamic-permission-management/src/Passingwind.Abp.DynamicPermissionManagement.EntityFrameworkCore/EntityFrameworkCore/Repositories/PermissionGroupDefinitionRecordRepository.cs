using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using IPermissionGroupDefinitionRecordRepository = Passingwind.Abp.DynamicPermissionManagement.Permissions.IPermissionGroupDefinitionRecordRepository;

namespace Passingwind.Abp.DynamicPermissionManagement.EntityFrameworkCore.Repositories;

public class PermissionGroupDefinitionRecordRepository : EfCoreRepository<IPermissionManagementDbContext, PermissionGroupDefinitionRecord, Guid>, IPermissionGroupDefinitionRecordRepository
{
    public PermissionGroupDefinitionRecordRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
