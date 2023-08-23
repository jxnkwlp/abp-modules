using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.MongoDB;
using IPermissionGroupDefinitionRecordRepository = Passingwind.Abp.DynamicPermissionManagement.Permissions.IPermissionGroupDefinitionRecordRepository;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB.Repositories;

public class PermissionGroupDefinitionRecordRepository : MongoDbRepository<IPermissionManagementMongoDbContext, PermissionGroupDefinitionRecord, Guid>,
    IPermissionGroupDefinitionRecordRepository
{
    public PermissionGroupDefinitionRecordRepository(IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
