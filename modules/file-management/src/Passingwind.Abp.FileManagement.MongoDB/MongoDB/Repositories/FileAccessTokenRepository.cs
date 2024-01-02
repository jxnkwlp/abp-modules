using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB.Repositories;

public class FileAccessTokenRepository : MongoDbRepository<FileManagementMongoDbContext, FileAccessToken, Guid>, IFileAccessTokenRepository
{
    public FileAccessTokenRepository(IMongoDbContextProvider<FileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(Guid containerId, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .Where(x => x.ContainerId == containerId)
            .As<IMongoQueryable<FileContainer>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<FileAccessToken>> GetListAsync(Guid containerId, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .Where(x => x.ContainerId == containerId)
            .As<IMongoQueryable<FileAccessToken>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileAccessToken>> GetPagedListAsync(int skipCount, int maxResultCount, Guid containerId, Guid? fileId = null, Guid? userId = null, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .Where(x => x.ContainerId == containerId)
            .OrderByDescending(x => x.CreationTime)
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<FileAccessToken>>()
            .ToListAsync(cancellationToken);
    }
}
