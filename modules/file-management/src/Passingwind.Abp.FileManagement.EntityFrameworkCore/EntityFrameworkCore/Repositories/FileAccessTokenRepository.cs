using System;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileAccessTokenRepository : EfCoreRepository<FileManagementDbContext, FileAccessToken, Guid>, IFileAccessTokenRepository
{
    public FileAccessTokenRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
