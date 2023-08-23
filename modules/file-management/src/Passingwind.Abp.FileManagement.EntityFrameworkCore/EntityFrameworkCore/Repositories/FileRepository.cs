using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Passingwind.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore.Repositories;

public class FileRepository : EfCoreRepository<FileManagementDbContext, File, Guid>, IFileRepository
{
    public FileRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> GetCountAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<File>> GetListAsync(string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string? filter = null, Guid? containerId = null, Guid? parentId = null, bool? isDirectory = null, string? sorting = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.FileName.Contains(filter!))
            .WhereIf(containerId.HasValue, x => x.ContainerId == containerId)
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .WhereIf(isDirectory.HasValue, x => x.IsDirectory == isDirectory)
            .OrderBy(sorting ?? nameof(File.CreationTime) + " desc")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsFileNameExistsAsync(Guid containerId, string fileName, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        var dbset = await GetDbSetAsync();
        return await dbset
            .Where(x => x.ContainerId == containerId && x.FileName == fileName && x.ParentId == parentId)
            .AnyAsync(cancellationToken);
    }
}
