using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB.Repositories;

public class FileRepository : MongoDbRepository<FileManagementMongoDbContext, FileItem, Guid>, IFileRepository
{
    public FileRepository(IMongoDbContextProvider<FileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<FileItem?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<FileItem>>()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<FileItem>>()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(FileItem), fileName);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<FileItem>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<FileItem>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .OrderBy(sorting ?? nameof(FileItem.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<FileItem>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();
        return await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .Where(x => x.ContainerId == containerId && x.FileName == fileName)
            .As<IMongoQueryable<FileItem>>()
            .AnyAsync(cancellationToken);
    }
}
