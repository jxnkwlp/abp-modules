using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileAccessTokenRepository : EfCoreRepository<FileManagementDbContext, FileAccessToken, Guid>, IFileAccessTokenRepository
{
    public FileAccessTokenRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(Guid? containerId = null, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<FileAccessToken>> GetListAsync(Guid? containerId = null, Guid? fileId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileAccessToken>> GetPagedListAsync(int skipCount, int maxResultCount, Guid? containerId = null, Guid? fileId = null, Guid? userId = null, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .WhereIf(fileId.HasValue, x => x.FileId == fileId)
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .OrderByDescending(x => x.CreationTime)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }
}
