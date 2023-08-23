using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.MongoDB;
using IPermissionDefinitionRecordRepository = Passingwind.Abp.DynamicPermissionManagement.Permissions.IPermissionDefinitionRecordRepository;

namespace Passingwind.Abp.DynamicPermissionManagement.MongoDB.Repositories;

public class PermissionDefinitionRecordRepository : MongoDbRepository<IPermissionManagementMongoDbContext, PermissionDefinitionRecord, Guid>,
    IPermissionDefinitionRecordRepository
{
    public PermissionDefinitionRecordRepository(IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
