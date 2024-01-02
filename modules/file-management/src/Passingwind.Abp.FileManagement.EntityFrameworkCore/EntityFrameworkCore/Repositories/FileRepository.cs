using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileRepository : EfCoreRepository<FileManagementDbContext, FileItem, Guid>, IFileRepository
{
    public FileRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<FileItem?> FindByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);
    }

    public virtual async Task<FileItem> GetByNameAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        var entity = await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .FirstOrDefaultAsync(x => x.ContainerId == containerId && x.FileName == fileName, cancellationToken: cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(FileItem), fileName);
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<FileItem>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .OrderBy(sorting ?? nameof(FileItem.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();

        return await dbset
            .Where(x => x.ParentId == (parentId ?? Guid.Empty))
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .Where(x => x.ContainerId == containerId && x.FileName == fileName)
            .AnyAsync(cancellationToken);
    }
}
