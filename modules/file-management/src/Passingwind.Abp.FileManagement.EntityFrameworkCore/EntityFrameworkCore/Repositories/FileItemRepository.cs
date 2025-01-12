using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileItemRepository : EfCoreRepository<FileManagementDbContext, FileItem, Guid>, IFileItemRepository
{
    public FileItemRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    protected async Task<IQueryable<FileItem>> BuildQueryableAsync(Guid? containerId = null, string? filter = null, Guid? parentId = null, bool? isDirectory = null)
    {
        var dbset = await GetDbSetAsync();

        // https://github.com/dotnet/efcore/issues/35095
        var pid = (parentId ?? Guid.Empty);

        return dbset
            .IncludeAll()
            .Where(x => x.ParentId == pid)
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            ;
    }

    public virtual async Task<FileItem?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, parentId: parentId, isDirectory: isDirectory);

        return await query
            .IncludeAll()
            .FirstOrDefaultAsync(x => x.FileName == fileName, cancellationToken: cancellationToken);
    }

    public async Task<FileItem?> FindByPathAsync(Guid containerId, string filePath, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .IncludeAll()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.Path != null && x.Path.FullPath == filePath, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, parentId: parentId, isDirectory: isDirectory);

        var entity = await query
            .IncludeAll()
            .FirstOrDefaultAsync(x => x.FileName == fileName, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(FileItem), fileName);
    }

    public async Task<FileItem> GetByPathAsync(Guid containerId, string filePath, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset
            .IncludeAll()
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.Path != null && x.Path.FullPath == filePath, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(FileItem), filePath);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, filter: filter, parentId: parentId, isDirectory: isDirectory);

        return await query.LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, filter: filter, parentId: parentId, isDirectory: isDirectory);

        return await query.IncludeDetails(includeDetails)
                          .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .Where(x => ids.Contains(x.Id))
            .IncludeDetails(includeDetails)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, filter: filter, parentId: parentId, isDirectory: isDirectory);

        return await query
            .IncludeDetails(includeDetails)
            .OrderBy(sorting ?? nameof(FileItem.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var query = await BuildQueryableAsync(containerId, parentId: parentId, isDirectory: isDirectory);

        return await query.Where(x => x.FileName == fileName)
            .AnyAsync(cancellationToken);
    }
}
