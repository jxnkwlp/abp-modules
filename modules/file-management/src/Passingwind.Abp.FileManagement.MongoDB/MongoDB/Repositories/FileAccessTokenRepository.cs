using System;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB.Repositories;

public class FileAccessTokenRepository : MongoDbRepository<FileManagementMongoDbContext, FileAccessToken, Guid>, IFileAccessTokenRepository
{
    public FileAccessTokenRepository(IMongoDbContextProvider<FileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
