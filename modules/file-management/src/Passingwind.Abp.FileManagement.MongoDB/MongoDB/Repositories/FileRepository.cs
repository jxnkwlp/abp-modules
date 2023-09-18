using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Passingwind.Abp.FileManagement.MongoDB.Repositories;

public class FileRepository : MongoDbRepository<FileManagementMongoDbContext, File, Guid>, IFileRepository
{
    public FileRepository(IMongoDbContextProvider<FileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<File?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .As<IMongoQueryable<File>>()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);
    }

    public async Task<File> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .As<IMongoQueryable<File>>()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException();
    }

    public async Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<File>>()
            .LongCountAsync(cancellationToken);
    }

    public async Task<List<File>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .As<IMongoQueryable<File>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<File>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public async Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .OrderBy(sorting ?? nameof(File.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<File>>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();
        return await query
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .Where(x => x.ContainerId == containerId && x.FileName == fileName)
            .As<IMongoQueryable<File>>()
            .AnyAsync(cancellationToken);
    }
}
