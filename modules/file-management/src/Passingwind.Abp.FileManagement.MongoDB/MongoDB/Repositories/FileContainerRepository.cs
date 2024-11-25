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

public class FileContainerRepository : MongoDbRepository<FileManagementMongoDbContext, FileContainer, Guid>, IFileContainerRepository
{
    public FileContainerRepository(IMongoDbContextProvider<FileManagementMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> CheckExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.AnyAsync(x => x.Id != fileContainer.Id && x.Name == fileContainer.Name, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .As<IMongoQueryable<FileContainer>>()
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<FileContainer?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException();
    }

    public virtual async Task<List<FileContainer>> GetListAsync(string? filter = null, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .As<IMongoQueryable<FileContainer>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileContainer>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .OrderBy(sorting ?? nameof(FileContainer.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .As<IMongoQueryable<FileContainer>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task IncrementFileCountAsync(string name, int adjustment = 1, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<FileContainer>();

        var update = new UpdateDefinitionBuilder<FileContainer>();

        await collection.UpdateOneAsync(x => x.Name == name, update.Inc(x => x.FilesCount, adjustment), cancellationToken: cancellationToken);
    }
}
