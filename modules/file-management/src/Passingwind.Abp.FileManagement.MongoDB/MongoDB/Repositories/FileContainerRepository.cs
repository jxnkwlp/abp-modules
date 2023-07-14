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

    public async Task<bool> CheckExistsAsync(FileContainer fileContainer, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.AnyAsync(x => x.Name == fileContainer.Name, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(string? filter, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .As<IMongoQueryable<FileContainer>>()
            .LongCountAsync(cancellationToken);
    }

    public async Task<FileContainer?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
    }

    public async Task<FileContainer> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        var entity = await query.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException();
    }

    public virtual async Task<List<FileContainer>> GetListAsync(string? filter, Guid? userId = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .As<IMongoQueryable<FileContainer>>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileContainer>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter, Guid? userId = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await GetMongoQueryableAsync();

        return await query
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
            .WhereIf(userId.HasValue, x => x.CreatorId == userId)
            .PageBy(skipCount, maxResultCount)
            .OrderBy(sorting ?? nameof(FileContainer.CreationTime) + " desc")
            .As<IMongoQueryable<FileContainer>>()
            .ToListAsync(cancellationToken);
    }

}
